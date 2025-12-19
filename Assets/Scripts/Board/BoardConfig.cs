using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfig", menuName = "Minesweeper/Board Config")]
public class BoardConfig : ScriptableObject
{
    [Header("Board Size")]
    public int columns = 30;    // 보드의 가로 칸 수 (x)
    public int rows = 16;       // 보드의 세로 칸 수 (y)

    [Header("Mines")]
    public int mineCount = 99;  // 전체 지뢰 개수
}