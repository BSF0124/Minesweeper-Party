using UnityEngine;

public static class Define
{
    // 숫자 색
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

    // 셀의 상태(0: 안열림, 1: 열림, 2: 깃발 표시)
    public enum CellState
    {
        Unopened,
        Opened,
        Flagged,
        // MineIdle,
        // MineExploded
    }

    // 8방향 탐색용
    public static readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
    public static readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
}