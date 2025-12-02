using UnityEngine;

public class Cell : MonoBehaviour
{
    public Sprite[] cellSprite;
    public int cellType;

    public void Init(int cellType)
    {
        this.cellType = cellType;
    }
}
