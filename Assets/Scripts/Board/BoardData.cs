// 보드의 논리 데이터만을 보관하는 순수 데이터 클래스
// 렌더링 및 데이터 입력 로직을 포함하지 않음

public class BoardData
{
    // 보드 크기(읽기 전용)
    public int columns { get; private set; }
    public int rows { get; private set; }

    // 실제 보드 데이터
    // -1 : 지뢰
    // 0~8 : 주변 지뢰 개수
    public int[,] board;

    public BoardData(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        board = new int[columns, rows];
    }

    // 해당 좌표가 지뢰인지 여부 반환
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

    // 보드 범위 체크 (Out of Range 방지)
    public bool IsInside(int column, int row)
    {
        return column >= 0 && column < columns && row >= 0 && row < rows;
    }
}
