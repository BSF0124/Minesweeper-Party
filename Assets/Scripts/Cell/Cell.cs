using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Text text;
    [SerializeField] private CellSprite cellSprite;

    public static event Action<Cell> OnLeftClick;   // 좌클릭 이벤트
    public static event Action<Cell> OnRightClick;  // 우클릭 이벤트

    public int column { get; private set; }                 // column id
    public int row { get; private set; }                    // row id
    public bool isMine { get; private set; }                // true: 지뢰 O, false: 지뢰 X
    public int aroundMineCount { get; private set; }        // 주변 지뢰 개수
    public Define.CellState cellState { get; private set; } // 셀 상태

    // 셀 초기화
    // value == -1 -> 지뢰
    // value >= 0 -> 주변 지뢰 개수
    public void Init(int column, int row, int value)
    {
        this.column = column;
        this.row = row;

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

    // 셀을 여는 핵심 로직
    // explodeMine == true -> 클릭으로 인한 지뢰 폭발
    // explodeMine == false -> 게임 종료 후 표시용
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
}
