using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* BoardManager
 * 보드 전체의 게임 규칙을 관리하는 컨트롤러
 * - 보드 생성 타이밍 제어(첫 클릭 보호)
 * - 셀 클릭 이벤트 처리
 * - 셀 오픈 규칙
 * - 게임 오버 / 클리어 판정
 */
public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig boardConfig;   // 보드 설정 데이터
    [SerializeField] private GameObject cellPrefab;     // 셀 오브젝트 생성용 프리팹

    private BoardData boardData;    // 지뢰 및 숫자 정보를 담는 논리 보드 데이터
    private Cell[,] cells;          // 생성된 셀 오브젝트 참조 배열

    public int openedSafeCellCount = 0;     // 열린 안전 셀 개수
    public int totalSafeCellCount = 480;    // 전체 안전 셀 개수

    bool isBoardGenerated = false;  // 첫 클릭 이후 보드 생성 여부

    private int flagCount = 0;  // 현재 보드에 표시된 깃발 수
    // UI에서 참조하는 남은 지뢰 수
    public int RemainingMineCount => boardConfig.mineCount - flagCount;
    // 남은 지뢰 수 변경 시 UI 갱신을 위한 이벤트
    public event Action<int> OnRemainingMineCountChanged;

    // 메인 메뉴에서 전돨된 게임 시작 정보를 확인하고
    // 유효하지 않을 경우 메인 메뉴로 돌아감
    private void Awake()
    {
        if(!TryResolveGameStartContext())
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        GameStartContext.Consume();
    }

    // 보드 초기화 수행
    private void Start()
    {
        InitBoard();
    }

    // GameStartContext에 저장된 모드/난이도를 기반으로
    // 사용할 BoardConfig를 로드
    private bool TryResolveGameStartContext()
    {
        if(!GameStartContext.TryGet(out var mode, out var difficulty))
            return false;

        boardConfig = LoadBoardConfig(mode, difficulty);
        return boardConfig != null;
    }


    // 선택된 게임 모드 및 난이도에 따라
    // Resource 폴더에서 적절한 BoardConfig를 로드
    private BoardConfig LoadBoardConfig(Define.GameMode gameMode, Define.Difficulty difficulty)
    {
        string path;
        if(gameMode == Define.GameMode.Classic)
            path = $"BoardConfig/Board_{difficulty}";
        else
            path = $"BoardConfig/Board_{gameMode}";

        BoardConfig config = Resources.Load<BoardConfig>(path);

        if(config == null)
            Debug.LogError($"BoardConfig 로드 실패: {path}");
        
        return config;
    }

    // 게임 시작 시 또는 리셋 시 호출
    // - 셀 오브젝트 생성
    // - 보드 데이터 초기화
    // - 지뢰 생성 X
    private void InitBoard()
    {
        boardData = new BoardData(boardConfig.columns, boardConfig.rows);

        isBoardGenerated = false;

        openedSafeCellCount = 0;
        totalSafeCellCount = boardConfig.columns * boardConfig.rows - boardConfig.mineCount;
        
        flagCount = 0;
        OnRemainingMineCountChanged?.Invoke(RemainingMineCount);

        BoardRenderer renderer = new BoardRenderer(cellPrefab, transform);
        cells = renderer.Render(boardData);
    }


    // 현재 보드를 초기화하고 다시 생성
    public void ResetBoard()
    {
        GameManager.Instance.ResetTimer();

        foreach(Transform cellObjects in transform)
            Destroy(cellObjects.gameObject);

        boardData = null;
        cells = null;

        InitBoard();
    }

    // 첫 클릭 좌표를 기준으로 보드 데이터 생성
    // 첫 클릭 보호 규칙 적용
    private void GenerateBoard(int click_col, int click_row)
    {
        BoardGenerator generator = new BoardGenerator(boardData, boardConfig.mineCount, click_col, click_row);
        generator.GenerateBoard();

        // 생성된 데이터를 각 Cell에 반영
        for(int col = 0; col < boardData.columns; col++)
            for(int row = 0; row < boardData.rows; row++)
                cells[col,row].SetValue(boardData.GetValue(col, row));
    }

    // Cell 이벤트 등록 / 해제
    private void OnEnable()
    {
        Cell.OnLeftClick += HandleLeftClick;
        Cell.OnRightClick += HandleRightClick;
        Cell.OnFlagToggled += HandleFlagToggled;
    }

    private void OnDisable()
    {
        Cell.OnLeftClick -= HandleLeftClick;
        Cell.OnRightClick -= HandleRightClick;
        Cell.OnFlagToggled -= HandleFlagToggled;
    }

    // 좌클릭 시 셀 상태에 따라
    // - 보드 생성
    // - 셀 오픈
    // - 연쇄 오픈
    // 을 분기 처리한다.
    private void HandleLeftClick(Cell cell)
    {
        if (GameManager.Instance.gameState != Define.GameState.Playing)
            return;
        
        if(cell.cellState == Define.CellState.Flagged)
            return;

        if(cell.cellState == Define.CellState.Opened)
        {
            OpenAdjacentCells(cell);
            return;
        }

        // 첫 클릭 시점에만 보드 데이터 생성
        if(!isBoardGenerated)
        {
            GenerateBoard(cell.column, cell.row);
            isBoardGenerated = true;
            GameManager.Instance.StartTimer();
        }

        TryOpenCell(cell);
    }

    // 우클릭 시 셀의 깃발 상태를 토글
    private void HandleRightClick(Cell cell)
    {
        if (GameManager.Instance.gameState != Define.GameState.Playing)
            return;
            
        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }


    // 셀의 깃발 상태 변경에 따라
    // 남은 지뢰 수를 갱신
    private void HandleFlagToggled(Cell cell, bool isFlagged)
    {
        if(GameManager.Instance.gameState != Define.GameState.Playing)
            return;
        
        flagCount += isFlagged ? 1 : -1;

        OnRemainingMineCountChanged?.Invoke(RemainingMineCount);
    }

    // 모든 셀 오픈이 반드시 거치는 유일한 진입점
    private void TryOpenCell(Cell cell)
    {
        if(cell.cellState != Define.CellState.Unopened) 
            return;

        cell.OpenCell(cell.isMine);

        // 지뢰 클릭 -> 즉시 게임 오버 처리
        if(cell.isMine)
        {
            GameManager.Instance.GameOver();
            RevealAllMines();
            return;
        }

        // 주변 지뢰가 없는 셀 -> 연쇄 오픈
        if(cell.aroundMineCount == 0)
            OpenConnectedCells(cell);

        // 클리어 조건 검사
        if(++openedSafeCellCount == totalSafeCellCount)
        {
            GameManager.Instance.GameClear();
            RevealAllMines();
        }
    }

    // 열린 숫자 셀에서
    // 주변 깃발 수가 숫자와 같을 경우 인접 셀을 자동 오픈
    private void OpenAdjacentCells(Cell cell)
    {
        List<Cell> adjacentCells = new List<Cell>();
        int flagCount = 0;

        for(int dir = 0; dir < 8; dir++)
        {
            int nx = cell.column + Define.dx[dir];
            int ny = cell.row + Define.dy[dir];

            if(!boardData.IsInside(nx,ny)) 
                continue;

            Cell nextCell = cells[nx,ny];

            if(nextCell.cellState == Define.CellState.Flagged) 
                flagCount++;

            else if(nextCell.cellState == Define.CellState.Unopened) 
                adjacentCells.Add(nextCell);
        }

        if(flagCount != cell.aroundMineCount)
            return;
        
        foreach(var adjacentCell in adjacentCells)
            TryOpenCell(adjacentCell);
    }

    // 주변 지뢰 개수가 0인 셀을 기준으로
    // 인접 셀을 연쇄적으로 오픈
    private void OpenConnectedCells(Cell startCell)
    {
        for(int dir = 0; dir < 8; dir++)
        {
            int nx = startCell.column + Define.dx[dir];
            int ny = startCell.row + Define.dy[dir];

            if(!boardData.IsInside(nx,ny)) 
                continue;

            Cell nextCell = cells[nx,ny];

            if(nextCell.cellState != Define.CellState.Unopened) 
                continue;

            if(nextCell.isMine) 
                continue;

            TryOpenCell(nextCell);
        }
    }

    // 게임 종료 시 모든 지뢰를 공개하고
    // 잘못 표시된 깃발을 시각적으로 표시
    public void RevealAllMines()
    {
        List<Cell> mineCells = new List<Cell>();
        List<Cell> flagCells = new List<Cell>();

        foreach(Cell cell in cells)
        {
            if(cell.isMine && cell.cellState == Define.CellState.Unopened)
                mineCells.Add(cell);
            if(!cell.isMine && cell.cellState == Define.CellState.Flagged)
                flagCells.Add(cell);
        }
        
        foreach(Cell cell in mineCells)
            cell.OpenCell(false);
        
        foreach(Cell cell in flagCells)
            cell.ShowWrongFlag();
    }

    // 디버깅용 단축키
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RevealAllMines();
        }
    }
}