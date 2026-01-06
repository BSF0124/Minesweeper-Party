using UnityEngine;

/*
 * Define
 * 게임 전반에서 공통으로 참조되는 정의 모음 클래스
 * - 게임 상태, 모드, 난이도 등의 열거형
 * - 셀 상태 정의
 * - 숫자 표시 색상
 * - 보드 탐색에 사용되는 방향 오프셋
 */
public static class Define
{
    // 선택한 게임 모드
    public enum GameMode
    {
        None,
        Classic,
        Coop,
        Versus
    }

    // Classic 게임 모드의 난이도
    public enum Difficulty
    {
        None,
        Easy,
        Normal,
        Hard
    }

    // 현재 게임의 진행 상태를 나타냄
    public enum GameState
    {
        Playing,    // 게임 진행 중
        GameOver,   // 게임 오버
        Cleared     // 게임 클리어
    }

    // CellState : 셀의 논리적 상태
    public enum CellState
    {
        Unopened,   // 열리지 않은 상태
        Opened,     // 열려있는 상태
        Flagged     // 깃발이 표시된 상태
    }

    // 셀에 표시되는 숫자(주변 지뢰 개수)에 대응하는 색상 배열
    // 인덱스 = 주변 지뢰 개수
    public static readonly Color32[] NumberColors =
    {
        new Color32(0,0,0,0),           // 0 (표시하지 않음)
        new Color32(0,0,255,255),       // 1 #0000FF
        new Color32(0,128,0,255),       // 2 #008000
        new Color32(255,0,0,255),       // 3 #FF0000
        new Color32(0,0,128,255),       // 4 #000080
        new Color32(128,0,0,255),       // 5 #800000
        new Color32(0,128,128,255),     // 6 #008080
        new Color32(0,0,0,255),         // 7 #000000
        new Color32(128,128,128,255)    // 8 #808080
    };

    // 보드에서 특정 셀 기준 8방향 탐색을 위한 좌표 오프셋
    // (좌상 -> 상 -> 우상 -> 좌 -> 우 -> 좌하 -> 하 -> 우하)
    public static readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
    public static readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
}