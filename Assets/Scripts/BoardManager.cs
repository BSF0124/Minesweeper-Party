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
}