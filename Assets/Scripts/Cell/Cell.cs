using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Text text;
    [SerializeField] private CellSprite cellSprite;

    public static event Action<Cell> OnLeftClick;
    public static event Action<Cell> OnRightClick;

    public int column { get; private set; }
    public int row { get; private set; }
    public bool isMine { get; private set; }
    public int aroundMineCount { get; private set; }
    public Define.CellState cellState { get; private set; }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick?.Invoke(this);

        if(eventData.button == PointerEventData.InputButton.Right)
            OnRightClick?.Invoke(this);
    }

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
