using UnityEngine;

public static class Define
{
    public enum GameMode
    {
        None,
        Single,
        Coop,
        Versus
    }

    public enum Difficulty
    {
        None,
        Easy,
        Normal,
        Hard
    }

    // GameState : 현재 게임 상태
    public enum GameState
    {
        Playing,
        GameOver,
        Cleared
    }

    // CellState : 셀의 논리적 상태
    // Unopened : 아직 열리지 않음
    // Opened   : 열림
    // Flagged  : 깃발 표시 
    public enum CellState
    {
        Unopened = 0,
        Opened,
        Flagged
    }

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

    // 8방향 탐색용 오프셋 배열
    // (좌상 -> 상 -> 우상 -> 좌 -> 우 -> 좌하 -> 하 -> 우하)
    public static readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
    public static readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
}