using UnityEngine;

[CreateAssetMenu(fileName = "CellSprite", menuName = "Minesweeper/Cell Sprite")]
public class CellSprite : ScriptableObject
{
    public Sprite unopened;     // 0
    public Sprite opened;       // 1
    public Sprite flag;         // 2
    public Sprite mineIdle;     // 3
    public Sprite mineExploded; // 4
}
