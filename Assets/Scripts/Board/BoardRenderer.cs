using UnityEngine;

/* BoardRenderer
 * BoardData 크기를 기준으로
 * 셀 오브젝트를 화면에 배치하는 역할
 * 게임 규칙에는 관여하지 않음
 */
public class BoardRenderer
{
    private GameObject cellPrefab;  // 생설할 셀 프리팹
    private Transform parent;       // 셀들의 부모 오브젝트

    public BoardRenderer(GameObject cellPrefab, Transform parent)
    {
        this.cellPrefab = cellPrefab;
        this.parent = parent;
    }

    // 셀 오브젝트 생성 및 배치
    public Cell[,] Render(BoardData data)
    {
        Cell[,] cells = new Cell[data.columns, data.rows];
        Vector3 startPos = CalculateStartPosition(data.columns, data.rows);

        for(int col = 0; col < data.columns; col++)
        {
            for(int row = 0; row < data.rows; row++)
            {
                // row는 아래 방향으로 증가하므로 y좌표는 감소
                Vector3 worldPos = new Vector3(
                    startPos.x + col,
                    startPos.y - row,
                    0f
                );

                GameObject cellObj = Object.Instantiate(cellPrefab, worldPos, Quaternion.identity, parent);

                Cell cell = cellObj.GetComponent<Cell>();
                cell.Init(col, row);
                cells[col,row] = cell;
            }
        }
        return cells;
    }

    // 보드 중앙 정렬을 위한 시작 위치 계산
    private Vector3 CalculateStartPosition(int columns, int rows)
    {
        float startX = -(columns - 1) * 0.5f;
        float startY = (rows -1) * 0.5f;

        return new Vector3(startX, startY, 0f);
    }
}
