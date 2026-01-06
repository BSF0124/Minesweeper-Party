using UnityEngine;

/*
 * BoardConfig
 * 보드 크기와 지뢰 개수를 설정하기 위한 데이터 컨테이너
 * ScriptableObject로 만들어져 에디터에서 모드 및 난이도 설정 용도로 사용됨
 */
[CreateAssetMenu(fileName = "BoardConfig", menuName = "Minesweeper/Board Config")]
public class BoardConfig : ScriptableObject
{
    [Header("Board Size")]
    public int columns = 30;    // 보드의 가로 칸 수
    public int rows = 16;       // 보드의 세로 칸 수

    [Header("Mines")]
    public int mineCount = 99;  // 보드 전체에 배치될 지뢰 개수
}