using UnityEngine;

public static class Define
{
    public static readonly Color32[] NumberColors =
    {
        new Color32(0,0,0,0),
        new Color32(0,0,255,255),       // #0000FF
        new Color32(0,128,0,255),       // #008000
        new Color32(255,0,0,255),       // #FF0000
        new Color32(0,0,128,255),       // #000080
        new Color32(128,0,0,255),       // #800000
        new Color32(0,128,128,255),     // #008080
        new Color32(0,0,0,255),         // #000000
        new Color32(128,128,128,255)    // #808080
    };

    public enum CellState
    {
        Unopened,
        Opened,
        Flagged,
        // MineIdle,
        // MineExploded
    }
}