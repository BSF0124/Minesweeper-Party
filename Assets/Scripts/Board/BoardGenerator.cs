using UnityEngine;

public class BoardGenerator
{
    private BoardData data;
    private int mineCount;

    // 8방향 탐색용
    private static readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

    public BoardGenerator(BoardData data, int mineCount)
    {
        this.data = data;
        this.mineCount = mineCount;
    }

    public void GenerateBoard()
    {
        PlaceMines();
        CalculateNumbers();
    }

    private void PlaceMines()
    {
        int placed = 0;

        while(placed <= mineCount)
        {
            int col = Random.Range(0, data.columns);
            int row = Random.Range(0, data.rows);

            if(data.IsMine(col, row))
                continue;
            
            data.SetMine(col, row);
            placed++;
        }
    }

    private void CalculateNumbers()
    {
        for(int col = 0; col < data.columns; col++)
        {
            for(int row = 0; row < data.rows; row++)
            {
                if(data.IsMine(col, row))
                    continue;
                
                int count = 0;

                for(int dir = 0; dir < 8; dir++)
                {
                    int nc = col + dx[dir];
                    int nr = row + dy[dir];

                    if(!data.IsInside(nc, nr))
                        continue;
                    
                    if(data.IsMine(nc, nr))
                        count++;
                }

                data.SetNumber(col, row, count);
            }
        }
    }
}
