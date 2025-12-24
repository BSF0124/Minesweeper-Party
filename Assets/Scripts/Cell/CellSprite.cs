using UnityEngine;

// 셀 상태별 스프라이트 모음 ScriptableObject
// 아트 리소스 분리 목적
[CreateAssetMenu(fileName = "CellSprite", menuName = "Minesweeper/Cell Sprite")]
public class CellSprite : ScriptableObject
{
    public Sprite unopened;
    public Sprite opened;
    public Sprite flag;
    public Sprite mineIdle;
    public Sprite mineExploded;
    public Sprite wrongFlag;
}
