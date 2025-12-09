using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig boardConfig;
    [SerializeField] private GameObject cellPrefab;

    private BoardData boardData;

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
        renderer.Render(boardData);
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
            cell.OpenCell();
        
        if(cell.cellState == Define.CellState.Opened)
        {
            // 빈칸 오픈 로직 추가
        }
    }

    private void HandleRightClick(Cell cell)
    {
        // Debug.Log($"Right Click: {cell.column}, {cell.row}");

        if(cell.cellState == Define.CellState.Opened)
            return;

        cell.ToggleFlag();
    }
}