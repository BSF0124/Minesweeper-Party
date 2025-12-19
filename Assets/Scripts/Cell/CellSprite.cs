using UnityEngine;

// 셀 상태별 스프라이트 모음 ScriptableObject
// 아트 리소스 분리 목적
[CreateAssetMenu(fileName = "CellSprite", menuName = "Minesweeper/Cell Sprite")]
public class CellSprite : ScriptableObject
{
    public Sprite unopened;     // 0
    public Sprite opened;       // 1
    public Sprite flag;         // 2
    public Sprite mineIdle;     // 3
    public Sprite mineExploded; // 4
}
