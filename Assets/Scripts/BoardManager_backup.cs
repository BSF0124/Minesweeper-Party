using UnityEngine;

public class BoardManager_backup : MonoBehaviour
{
    // 셀 프리팹
    [SerializeField] private GameObject cellPrefab;

    // 보드 크기(가로 30, 세로 16)
    int board_Column = 30;
    int board_Row = 16;

    // 보드 데이터
    // -1 = 지뢰, 0~8 = 주변 지뢰 개수
    int[,] board;

    // 주변 8방향 탐색용 (dx, dy)
    int[] dx = new int[8]{-1,0,1,-1,1,-1,0,1};
    int[] dy = new int[8]{-1,-1,-1,0,0,1,1,1};

    private void Start()
    {
        Init();
    }

    // 보드 초기화
    private void Init()
    {
        // 배열 초기화
        board = new int[board_Column, board_Row];

        // 지뢰를 무작위로 배치
        SetRandomMines();

        // 모든 칸의 주변 지뢰 계산
        for(int col = 0; col < board_Column; col++)
        {
            for(int row = 0; row < board_Row; row++)
            {
                // 해당 칸이 지뢰가 아닌 경우에만 주변 지뢰 계산
                if(board[col,row] != -1) CalculateAdjacentMines(col, row);
            }
        }

        // 보드에 실제 셀 오브젝트 생성
        InstantiateCells();
    }

    // 무작위 지뢰 생성
    private void SetRandomMines()
    {
        int mineCount = 0;

        // 총 99개의 지뢰를 배치
        while(mineCount < 99)
        {
            int x = Random.Range(0, board_Column);
            int y = Random.Range(0, board_Row);

            if(board[x,y] != -1)
            {
                board[x,y] = -1;
                mineCount++;
            }
        }
    }

    // (x, y) 위치 주변의 8칸에 있는 지뢰 개수 계산
    private void CalculateAdjacentMines(int x, int y)
    {
        int mineCount = 0;

        for(int dir = 0; dir < 8; dir++)
        {
            int nx = x + dx[dir];
            int ny = y + dy[dir];

            // 보드 범위를 벗어나면 무시
            if(nx < 0 || nx >= board_Column || ny < 0 || ny >= board_Row)
                continue;
            
            // 주변 칸이 지뢰이면 카운트 증가
            if(board[nx,ny] == -1) mineCount++;
        }

        // 해당 칸에 주변 지뢰 수 저장
        board[x,y] = mineCount;
    }

    // 셀 오브젝트 생성 및 배치
    private void InstantiateCells()
    {
        // 보드를 화면 중앙에 정렬하기 위한 시작 위치 계산
        Vector3 initPosition = new Vector3(-(board_Column/2), board_Row/2, 0);

        // 보드 크기가 짝수일 경우 정중앙 배치 보정
        if(board_Column % 2 == 0) initPosition.x += 0.5f;
        if(board_Row % 2 == 0) initPosition.y -= 0.5f;

        // 실제 셀 프리팹 생성 반복
        for(int i = 0; i < board_Column; i++)
        {
            for(int j = 0; j < board_Row; j++)
            {
                Instantiate(
                    cellPrefab, 
                    initPosition + new Vector3(i, -j ,0), 
                    Quaternion.identity, 
                    transform
                );
            }
        }
    }
}
