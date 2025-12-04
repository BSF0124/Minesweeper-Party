using UnityEngine;

public class Cell : MonoBehaviour
{
    public Sprite[] cellSprite;

    public int column { get; private set; }
    public int row { get; private set; }

    public bool isMine { get; private set; }
    public int aroundMineCount { get; private set; }


    public void Init(int column, int row, int value)
    {
        this.column = column;
        this.row = row;

        isMine = value == -1;
        aroundMineCount = isMine ? 0 : value;
    }
}
