using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/* Cell
단일 셀의 상태와 시각적 표현을 담당
- 클릭 이벤트 전달
- 스프라이트 / 텍스트 표시
*/
public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CellSprite cellSprite;

    public static event Action<Cell> OnLeftClick;   // 좌클릭 이벤트
    public static event Action<Cell> OnRightClick;  // 우클릭 이벤트

    public int column { get; private set; }                 // column id
    public int row { get; private set; }                    // row id
    public bool isMine { get; private set; }                // true: 지뢰 O, false: 지뢰 X
    public int aroundMineCount { get; private set; }        // 주변 지뢰 개수
    public Define.CellState cellState { get; private set; } // 셀 상태

    // 셀 초기화
    public void Init(int column, int row)
    {
        this.column = column;
        this.row = row;
    }

    // 셀 데이터 설정 (보드 데이터 생성 이후 호출)
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

    // Unity EventSystem 기반 마우스 클릭 처리
    // BoardManager로 이벤트 전달
    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if(eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick?.Invoke(this);

        // 우클릭
        if(eventData.button == PointerEventData.InputButton.Right)
            OnRightClick?.Invoke(this);
    }

    // 깃발 표시 토글
    public void ToggleFlag()
    {
        if(cellState == Define.CellState.Flagged)
        {
            cellState = Define.CellState.Unopened;
            sr.sprite = cellSprite.unopened;
        }
        else if(cellState == Define.CellState.Unopened)
        {
            cellState = Define.CellState.Flagged;
            sr.sprite = cellSprite.flag;
        }
    }

    // 셀 오픈 처리
    // explodeMine == true : 플레이어 클릭으로 인한 폭발
    // explodeMine == false : 게임 종료 후 지뢰 공개 연출
    public void OpenCell(bool explodeMine = false)
    {
        if(cellState != Define.CellState.Unopened) 
            return;
        
        cellState = Define.CellState.Opened;
        
        if(isMine)
        {
            sr.sprite = explodeMine? cellSprite.mineExploded : cellSprite.mineIdle;
            return;
        }

        sr.sprite = cellSprite.opened;
        text.gameObject.SetActive(aroundMineCount > 0);
    }

    public void ShowWrongFlag()
    {
        sr.sprite = cellSprite.wrongFlag;
    }
}
