using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfig", menuName = "Minsweeper/Board Config")]
public class BoardConfig : ScriptableObject
{
    [Header("Board Size")]
    public int columns = 30;    // 가로 칸 수 (x)
    public int rows = 16;       // 세로 칸 수 (y)

    [Header("Mines")]
    public int mineCount = 99;
}