using UnityEngine;

/* BoardGenerator
보드 데이터(BoardData)를 실제 게임 규칙에 맞게 생성하는 클래스
- 첫 클릭 위치를 기준으로 지뢰 배치 예외 처리
- 지뢰 배치 이후 숫자 계산 수행
*/
public class BoardGenerator
{
    private BoardData data;
    private int mineCount;

    // 첫 클릭 위치 (이 좌표 + 주변 8칸에는 지뢰가 배치되지 않음)
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
    // 1. 지뢰 배치 (첫 클릭 보호 포함)
    // 2. 각 셀의 주변 지뢰 개수 계산
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

            // 첫 클릭 셀 + 주변 8칸 보호
            if(click_col - col >= -1 && click_col - col <= 1 && 
                click_row - row >= -1 && click_row - row <= 1)
                continue;

            if(data.IsMine(col, row))
                continue;
            
            data.SetMine(col, row);
            placed++;
        }
    }

    // 모든 셀을 순호하며 주변 8방향 지뢰 개수 계산
    // Define.dx / dy를 이용한 공통 방향 탐색 로직 사용
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
