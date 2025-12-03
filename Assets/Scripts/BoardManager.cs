using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    int board_Row = 30;
    int board_Column = 16;

    [HideInInspector] public int current_mines = 0;

    int[,] board;

    int[] dx = new int[8]{-1,0,1,-1,1,-1,0,1};
    int[] dy = new int[8]{-1,-1,-1,0,0,1,1,1};

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        board = new int[board_Row, board_Column];
        SetRandomMines();

        for(int row = 0; row < board_Row; row++)
        {
            for(int col = 0; col < board_Column; col++)
            {
                if(board[row,col] != -1) CalculateAdjacentMines(row, col);
            }
        }

        InstantiateCell();
    }

    private void SetRandomMines()
    {
        int mineCount = 0;
        while(mineCount < 99)
        {
            int x = Random.Range(0, board_Row);
            int y = Random.Range(0, board_Column);

            if(board[x,y] != -1)
            {
                board[x,y] = -1;
                mineCount++;
            }
        }
    }

    private void CalculateAdjacentMines(int x, int y)
    {
        int mineCount = 0;

        for(int dir = 0; dir < 8; dir++)
        {
            int nx = x + dx[dir];
            int ny = y + dy[dir];

            if(nx < 0 || nx >= board_Row || ny < 0 || ny >= board_Column)
                continue;
            
            if(board[nx,ny] == -1) mineCount++;
        }

        board[x,y] = mineCount;
    }

    private void InstantiateCell()
    {
        Vector3 initPosition = new Vector3(-(board_Row/2), board_Column/2, 0);

        if(board_Row % 2 == 0) initPosition.x += 0.5f;
        if(board_Column % 2 == 0) initPosition.y -= 0.5f;

        for(int i = 0; i < board_Column; i++)
        {
            for(int j = 0; j < board_Row; j++)
            {
                GameObject cell = Instantiate(cellPrefab, initPosition + new Vector3(j, -i ,0), Quaternion.identity, transform);
            }
        }
    }
}
