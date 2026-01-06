using UnityEngine;

/* CellSprite
 * 셀 상태에 따른 스프라이트 리소스 모음
 * ScriptableObject로 분리하여 아트 리소스 관리 목적
 */
[CreateAssetMenu(fileName = "CellSprite", menuName = "Minesweeper/Cell Sprite")]
public class CellSprite : ScriptableObject
{
    public Sprite unopened;     // 닫힌 상태
    public Sprite opened;       // 열린 상태
    public Sprite flag;         // 깃발 표시
    public Sprite mine_Default;     // 게임 종료 시 공개되는 지뢰
    public Sprite mine_Exploded; // 플레이어 클릭으로 폭발한 지뢰
    public Sprite wrongFlag;    // 잘못 표시된 깃발
}
