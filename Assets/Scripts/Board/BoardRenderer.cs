using UnityEngine;

public class BoardRenderer
{
    private GameObject cellPrefab;
    private Transform parent;

    public BoardRenderer(GameObject cellPrefab, Transform parent)
    {
        this.cellPrefab = cellPrefab;
        this.parent = parent;
    }

    public void Render(BoardData data)
    {
        Vector3 startPos = CalculateStartPosition(data.columns, data.rows);

        for(int col = 0; col < data.columns; col++)
        {
            for(int row = 0; row < data.rows; row++)
            {
                Vector3 worldPos = new Vector3(
                    startPos.x + col,
                    startPos.y - row,
                    0f
                );

                GameObject cellObj = Object.Instantiate(cellPrefab, worldPos, Quaternion.identity, parent);

                Cell cell = cellObj.GetComponent<Cell>();
                int value = data.GetValue(col, row);
                cell.Init(col, row, value);
            }
        }
    }

    private Vector3 CalculateStartPosition(int columns, int rows)
    {
        float startX = -(columns - 1) * 0.5f;
        float startY = (rows -1) * 0.5f;

        return new Vector3(startX, startY, 0f);
    }
}
