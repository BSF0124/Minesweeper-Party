using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/* Cell
 * 하나의 칸(Cell)을 표현하는 컴포넌트
 * - 셀의 좌표, 상태, 지뢰 여부를 보관
 * - 스프라이트 및 숫자 표시 담당
 * - 마우스 입력을 감지하여 이벤트로 전달
 * ※ 게임 규칙은 처리하지 않음
 */
public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer sr;     // 셀 스프라이트 표시용
    [SerializeField] private TextMeshProUGUI text;  // 주변 지뢰 개수 표시용 텍스트 
    [SerializeField] private CellSprite cellSprite; // 셀 상태별 스프라이트 묶음

    public static event Action<Cell> OnLeftClick;   // 좌클릭 시 이벤트
    public static event Action<Cell> OnRightClick;  // 우클릭 시 이벤트
    public static event Action<Cell,bool> OnFlagToggled;

    public int column { get; private set; }                 // 보드 상의 x 좌표
    public int row { get; private set; }                    // 보드 상의 y 좌표
    public bool isMine { get; private set; }                // 지뢰 여부
    public int aroundMineCount { get; private set; }        // 인접한 지뢰 개수
    public Define.CellState cellState { get; private set; } // 현재 셀 상태

    // 보드 생성 시 셀의 좌표 정보 초기화
    public void Init(int column, int row)
    {
        this.column = column;
        this.row = row;
    }

    // BoardData 생성 이후 호출
    // value == -1 : 지뢰
    // value >= 0 : 주변 지뢰 개수
    public void SetValue(int value)
    {
        isMine = value == -1;
        aroundMineCount = isMine ? 0 : value;
        text.text = $"{aroundMineCount}";
        text.color = Define.NumberColors[aroundMineCount];
        text.gameObject.SetActive(false);

        cellState = Define.CellState.Unopened;
        sr.sprite = cellSprite.unopened;
    }

    // Unity EventSystem 기반 클릭 처리
    // 실제 게임 로직은 처리하지 않고 이벤트만 전달
    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if(eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick?.Invoke(this);

        // 우클릭
        if(eventData.button == PointerEventData.InputButton.Right)
            OnRightClick?.Invoke(this);
    }

    // 깃발 상태를 토글
    // 열린 셀에는 적용되지 않음
    public void ToggleFlag()
    {
        if(cellState == Define.CellState.Opened)
            return;

        bool isNowFlagged = cellState != Define.CellState.Flagged;

        cellState = isNowFlagged ? Define.CellState.Flagged : Define.CellState.Unopened;
        sr.sprite = isNowFlagged ? cellSprite.flag : cellSprite.unopened;

        OnFlagToggled?.Invoke(this, isNowFlagged);
    }

    // 셀을 여는 처리
    // explodeMine : 플레이어 클릭으로 인한 지뢰 폭발 여부
    public void OpenCell(bool explodeMine = false)
    {
        if(cellState != Define.CellState.Unopened) 
            return;
        
        cellState = Define.CellState.Opened;
        
        if(isMine)
        {
            sr.sprite = explodeMine? cellSprite.mine_Exploded : cellSprite.mine_Default;
            return;
        }

        sr.sprite = cellSprite.opened;
        text.gameObject.SetActive(aroundMineCount > 0);
    }

    // 잘못 표시된 깃발 스프라이트 표시
    public void ShowWrongFlag()
    {
        sr.sprite = cellSprite.wrongFlag;
    }
}
