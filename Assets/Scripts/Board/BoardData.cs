/* BoardData
 * 보드의 논리 데이터만을 저장하는 클래스
 * - 지뢰 위치
 * - 각 셀의 주변 지뢰 개수
 * 렌더링 및 입력 처리 정보는 포함하지 않음
 */
public class BoardData
{
    public int columns { get; private set; }    // 보드 가로 크기
    public int rows { get; private set; }       // 보드 세로 크기

    // 실제 보드 데이터
    // -1 : 지뢰
    // 0~8 : 주변 지뢰 개수
    public int[,] board;

    // 보드 크기를 받아 데이터 배열 생성
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

    // 해당 좌표의 값 반환
    public int GetValue(int column, int row)
    {
        return board[column, row];
    }

    // 해당 좌표를 지뢰로 설정
    public void SetMine(int column, int row)
    {
        board[column, row] = -1;
    }

    // 해당 좌표에 숫자 설정
    public void SetNumber(int column, int row, int number)
    {
        board[column, row] = number;
    }

    // 좌표가 보드 범위 내인지 확인
    public bool IsInside(int column, int row)
    {
        return column >= 0 && column < columns && 
            row >= 0 && row < rows;
    }
}
