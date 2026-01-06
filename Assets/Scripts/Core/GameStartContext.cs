using UnityEngine;

public class GameStartContext
{
    private static Define.GameMode _mode = Define.GameMode.None;
    private static Define.Difficulty _difficulty = Define.Difficulty.None;

    public static Define.GameMode Mode => _mode;
    public static Define.Difficulty Difficulty => _difficulty;

    public static void SetClassic(Define.Difficulty difficulty)
    {
        _mode = Define.GameMode.Classic;
        _difficulty = difficulty;
    }

    public static bool TryGet(out Define.GameMode mode, out Define.Difficulty difficulty)
    {
        mode = _mode;
        difficulty = _difficulty;
        return _mode != Define.GameMode.None;
    }
    
    public static void Consume()
    {
        _mode = Define.GameMode.None;
        _difficulty = Define.Difficulty.None;
    }
}
