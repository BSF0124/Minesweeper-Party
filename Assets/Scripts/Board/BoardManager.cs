using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* BoardManager
 * 보드 전체의 게임 규칙을 관리하는 컨트롤러
 * - 셀 입력 처리
 * - 셀 오픈 규칙
 * - 게임 오버 / 클리어 판정
 */
public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig boardConfig;   // 보드 설정 데이터
    [SerializeField] private GameObject cellPrefab;     // 셀 프리팹

    private BoardData boardData;    // 논리 보드 데이터
    private Cell[,] cells;          // 셀 오브젝트 참조 배열

    public int openedSafeCellCount = 0;     // 열린 안전 셀 개수
    public int totalSafeCellCount = 480;    // 전체 안전 셀 개수

    bool isBoardGenerated = false;  // 첫 클릭 이후 보드 생성 여부

    private int flagCount = 0;  // 표시한 깃발 수
    public int RemainingMineCount => boardConfig.mineCount - flagCount; // 남은 지뢰 수
    public event Action<int> OnRemainingMineCountChanged; // 남은 지뢰 수 변경 이벤트

    private void Awake()
    {
        if(!TryResolveGameStartContext())
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        GameStartContext.Consume();
    }

    private void Start()
    {
        InitBoard();
    }

    private bool TryResolveGameStartContext()
    {
        if(!GameStartContext.TryGet(out var mode, out var difficulty))
            return false;

        boardConfig = LoadBoardConfig(mode, difficulty);
        return boardConfig != null;
    }

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

    // 게임 시작 시 셀 오브젝트만 생성
    // 실제 지뢰 데이터는 아직 생성하지 않음
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

    // 좌클릭 처리 규칙
    // 셀 상태에 따라 오픈, 연쇄 오픈, 보드 생성 분기
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

    private void HandleRightClick(Cell cell)
    {
        if (GameManager.Instance.gameState != Define.GameState.Playing)
            return;
            
        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }

    private void HandleFlagToggled(Cell cell, bool isFlagged)
    {
        if(GameManager.Instance.gameState != Define.GameState.Playing)
            return;
        
        flagCount += isFlagged ? 1 : -1;

        OnRemainingMineCountChanged?.Invoke(RemainingMineCount);
    }

    // 셀 오픈의 유일한 진입점
    // 모든 셀 오픈은 반드시 이 메서드를 통해 수행
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

    // 숫자가 있는 열린 셀에서
    // 주변 깃발 수가 숫자와 같을 경우 자동 오픈
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
    // DFS 방식으로 인접 셀을 연쇄 오픈
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
    // 현재는 DFS 구조로 만들었지만, 
    // 보드의 크기가 매우 커지거나 애니메이션 효과가 필요하거나 
    // 네트워크 동기화 문제 발생 시 BFS로 변경

    // 모든 지뢰 공개
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

    // 디버깅용
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RevealAllMines();
        }
    }
}