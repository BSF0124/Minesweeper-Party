using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig boardConfig;
    [SerializeField] private GameObject cellPrefab;

    private BoardData boardData;
    private Cell[,] cells;

    private void Start()
    {
        InitBoard();
    }

    // 보드 데이터 생성 -> 지뢰 생성 -> 화면 렌더링
    private void InitBoard()
    {
        boardData = new BoardData(boardConfig.columns, boardConfig.rows);

        BoardGenerator generator = new BoardGenerator(boardData, boardConfig.mineCount);
        generator.GenerateBoard();

        BoardRenderer renderer = new BoardRenderer(cellPrefab, transform);
        cells = renderer.Render(boardData);
    }

    private void OnEnable()
    {
        Cell.OnLeftClick += HandleLeftClick;
        Cell.OnRightClick += HandleRightClick;
    }

    private void OnDisable()
    {
        Cell.OnLeftClick -= HandleLeftClick;
        Cell.OnRightClick -= HandleRightClick;
    }

    // 좌클릭 처리 규칙
    // 1. 깃발 상태면 무시
    // 2. 이미 열린 셀 -> 주변 자동 오픈
    // 3. 일반 셀 -> 오픈 시도
    private void HandleLeftClick(Cell cell)
    {
        if (GameManager.Instance.gameState == Define.GameState.GameOver)
            return;
        
        if(cell.cellState == Define.CellState.Flagged)
            return;

        if(cell.cellState == Define.CellState.Opened)
        {
            OpenAdjacentCells(cell);
            return;
        }

        TryOpenCell(cell);
    }

    private void HandleRightClick(Cell cell)
    {
        if (GameManager.Instance.gameState == Define.GameState.GameOver)
            return;
            
        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }

    private void TryOpenCell(Cell cell)
    {
        if(cell.cellState != Define.CellState.Unopened) 
            return;

        cell.OpenCell(cell.isMine);

        // 지뢰 클릭 -> 게임 오버
        if(cell.isMine)
        {
            GameManager.Instance.GameOver();
            RevealAllMines();
            return;
        }

        // 빈 셀 -> 연쇄 오픈
        if(cell.aroundMineCount == 0)
        {
            OpenConnectedCells(cell);
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

            // 테스트용
            else if(nextCell.cellState == Define.CellState.Opened && nextCell.isMine) 
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
    // 연쇄적으로 셀을 열어가는 DFS 구조
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

    public void RevealAllMines()
    {
        List<Cell> mineCells = new List<Cell>();

        foreach(Cell cell in cells)
            if(cell.isMine && cell.cellState == Define.CellState.Unopened)
                mineCells.Add(cell);
        
        foreach(Cell cell in mineCells)
            cell.OpenCell(false);
    }
}