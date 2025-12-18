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

        if(cell.isMine)
        {
            cell.OpenCell(true);
            // 전체 지뢰 공개 로직 추가
            // 게임 오버 로직 추가
            return;
        }

        if(cell.cellState == Define.CellState.Unopened)
        {
            cell.OpenCell();
            if(cell.aroundMineCount == 0)
                OpenConnectedCells(cell);
        }
        
        if(cell.cellState == Define.CellState.Opened)
        {
            // 인접 칸 열기 로직
        }
    }

    private void HandleRightClick(Cell cell)
    {
        // Debug.Log($"Right Click: {cell.column}, {cell.row}");

        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }

    // 인접한 셀 오픈
    private void OpenAdjacentCells(Cell cell)
    {
        List<Cell> adjacentCells = new List<Cell>();
        int flag = 0;

        for(int dir = 0; dir < 8; dir++)
        {
            int nx = cell.column + Define.dx[dir];
            int ny = cell.row + Define.dy[dir];

            if(!boardData.IsInside(nx,ny)) continue;

            Cell nextCell = cells[nx,ny];

            if(nextCell.cellState == Define.CellState.Flagged) flag++;
            else if(nextCell.cellState == Define.CellState.Unopened) adjacentCells.Add(nextCell);
        }

        if(flag == cell.aroundMineCount)
            foreach(var adjacentCell in adjacentCells)
                adjacentCell.OpenCell();
    }

    // 빈 셀 연쇄 오픈
    private void OpenConnectedCells(Cell startCell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(startCell);

        while(queue.Count > 0)
        {
            Cell cur = queue.Dequeue();

            for(int dir = 0; dir < 8; dir++)
            {
                int nx = cur.column + Define.dx[dir];
                int ny = cur.row + Define.dy[dir];
                if(!boardData.IsInside(nx,ny)) continue;

                Cell nextCell = cells[nx,ny];
                if(nextCell.cellState != Define.CellState.Unopened) continue;
                if(nextCell.isMine) continue;

                cells[nx,ny].OpenCell();
                if(boardData.board[nx,ny] == 0) queue.Enqueue(nextCell);
            }
        }
    }
}