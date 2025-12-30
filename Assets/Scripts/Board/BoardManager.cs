using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* BoardManager
보드 전체의 플레이 규칙과 셀 상호작용을 관리하는 컨트롤러
- 입력 처리
- 셀 오픈 규칙
- 게임 오버 / 클리어 판정
*/
public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    private BoardConfig boardConfig;
    private BoardData boardData;
    private Cell[,] cells;

    // 첫 클릭 이후 보드 데이터 생성 여부
    bool isBoardGenerated = false;

    // 안전한 셀 오픈 카운트 (클리어 판정용)
    public int openedSafeCellCount = 0;
    public int totalSafeCellCount = 480;

    private int flagCount = 0;
    public int RemainingMineCount => boardConfig.mineCount - flagCount;

    public event Action<int> OnRemainingMineCountChanged;

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
        if(gameMode == Define.GameMode.Single)
            path = $"BoardConfig/Board_{difficulty}";
        else
            path = $"BoardConfig/Board_{gameMode}";

        BoardConfig config = Resources.Load<BoardConfig>(path);

        if(config == null)
            Debug.LogError($"BoardConfig 로드 실패: {path}");
        
        return config;
    }

    // 셀 오브젝트만 생성 (데이터는 아직 없음)
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

    // 첫 클릭 위치를 기준으로 보드 데이터 생성
    // 첫 클릭 보호 로직이 적용된 BoardGenerator 사용
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
    // 1. 게임 종료 상태면 무시
    // 2. 깃발 상태면 무시
    // 3. 이미 열린 셀 -> 주변 자동 오픈
    // 4. 첫 클릭이면 보드 데이터 생성
    // 5. 일반 셀 오픈 시도
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
    // 모든 오픈 로직은 반드시 이 메서드를 경유해야 함
    // - 지뢰 클릭 -> 게임 오버
    // - 빈 셀 -> 연쇄 오픈
    // - 안전 셀 카운트 증가 및 클리어 판정
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

    // 이미 열린 셀에서 주변 깃발 개수 == 숫자일 경우
    // 인접한 미오픈 셀들을 자동으로 오픈
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            RevealAllMines();
        }
    }
}