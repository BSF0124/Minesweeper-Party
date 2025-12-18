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

    private void HandleLeftClick(Cell cell)
    {
        // Debug.Log($"Left Click: {cell.column}, {cell.row}");

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
        // Debug.Log($"Right Click: {cell.column}, {cell.row}");

        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }

    private void TryOpenCell(Cell cell)
    {
        if(cell.cellState != Define.CellState.Unopened) 
            return;

        cell.OpenCell(cell.isMine);

        if(cell.isMine)
        {
            // 게임 오버
            return;
        }

        if(cell.aroundMineCount == 0)
        {
            OpenConnectedCells(cell);
        }
    }

    // 인접한 셀 오픈
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

    // 빈 셀 연쇄 오픈
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
}