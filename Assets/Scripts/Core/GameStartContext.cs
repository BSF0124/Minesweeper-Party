/*
 * GameStartContext
 * 메인 메뉴 → 게임 씬으로 넘어갈 때
 * 선택된 게임 모드와 난이도를 임시로 보관하기 위한 컨텍스트 클래스
 */
public class GameStartContext
{
    // 선택된 게임 모드
    private static Define.GameMode _mode = Define.GameMode.None;
    // 선택된 난이도
    private static Define.Difficulty _difficulty = Define.Difficulty.None;

    // 현재 설정된 게임 모드 외부 접근용 프로퍼티
    public static Define.GameMode Mode => _mode;
    // 현재 설정된 난이도 외부 접근용 프로퍼티
    public static Define.Difficulty Difficulty => _difficulty;

    // 클래식 게임 모드 시작을 위해 모드와 난이도를 설정
    public static void SetClassic(Define.Difficulty difficulty)
    {
        _mode = Define.GameMode.Classic;
        _difficulty = difficulty;
    }

    // 현재 설정된 게임 시작 정보를 가져옴
    // 반환값:
    // - true  : 유효한 게임 모드가 설정되어 있음
    // - false : 아무 모드도 선택되지 않음
    public static bool TryGet(out Define.GameMode mode, out Define.Difficulty difficulty)
    {
        mode = _mode;
        difficulty = _difficulty;
        return _mode != Define.GameMode.None;
    }

    // 게임 시작 정보 사용 후 호출되는 초기화 메서드
    // 씬 재진입 또는 중복 사용 방지를 위해 
    // 내부 상태를 기본값으로 되돌림
    public static void Consume()
    {
        _mode = Define.GameMode.None;
        _difficulty = Define.Difficulty.None;
    }
}
