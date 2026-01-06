using UnityEngine;

/* BoardGenerator
 * BoardData를 실제 게임 규칙에 맞게 생성하는 클래스
 * - 지뢰 배치
 * - 주변 지뢰 개수 계산
 */
public class BoardGenerator
{
    private BoardData data; // 생성 대상 보드 데이터
    private int mineCount;  // 배치할 지뢰 개수

    // 첫 클릭 위치
    // 해당 좌표와 주변 8칸에는 지뢰가 배치되지 않음
    private int click_col;
    private int click_row;

    public BoardGenerator(BoardData data, int mineCount, int click_col, int click_row)
    {
        this.data = data;
        this.mineCount = mineCount;
        this.click_col = click_col;
        this.click_row = click_row;
    }

    // 보드 생성 진입점
    public void GenerateBoard()
    {
        PlaceMines();
        CalculateNumbers();
    }

    // 랜덤 지뢰 배치
    // - 첫 클릭 셀과 그 주변 8칸은 제외
    // - 이미 지뢰가 있는 위치는 제외
    private void PlaceMines()
    {
        int placed = 0;

        while(placed < mineCount)
        {
            int col = Random.Range(0, data.columns);
            int row = Random.Range(0, data.rows);

            // 첫 클릭 셀과 인접 8칸 보호
            if(click_col - col >= -1 && click_col - col <= 1 && 
                click_row - row >= -1 && click_row - row <= 1)
                continue;

            if(data.IsMine(col, row))
                continue;
            
            data.SetMine(col, row);
            placed++;
        }
    }

    // 모든 셀의 주변 지뢰 개수 계산
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
