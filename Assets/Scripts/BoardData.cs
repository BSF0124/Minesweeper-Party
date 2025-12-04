public class BoardData
{
    public int columns { get; private set; }
    public int rows { get; private set; }

    public int[,] board;

    public BoardData(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        board = new int[columns, rows];
    }

    public bool IsMine(int column, int row)
    {
        return board[column, row] == -1;
    }

    public int GetValue(int column, int row)
    {
        return board[column, row];
    }

    public void SetMine(int column, int row)
    {
        board[column, row] = -1;
    }

    public void SetNumber(int column, int row, int number)
    {
        board[column, row] = number;
    }

    public bool IsInside(int column, int row)
    {
        return column >= 0 && column < columns && row >= 0 && row < rows;
    }
}
