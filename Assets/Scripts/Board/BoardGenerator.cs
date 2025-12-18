using UnityEngine;

public class BoardGenerator
{
    private BoardData data;
    private int mineCount;

    public BoardGenerator(BoardData data, int mineCount)
    {
        this.data = data;
        this.mineCount = mineCount;
    }

    // 보드 생성
    public void GenerateBoard()
    {
        PlaceMines();
        CalculateNumbers();
    }

    // 랜덤으로 지뢰 설정
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

    // 주변 지뢰 수 계산
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
                    int nc = col + Define.dx[dir];
                    int nr = row + Define.dy[dir];

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
