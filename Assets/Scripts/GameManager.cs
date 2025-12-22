using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Define.GameState gameState { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameState = Define.GameState.Playing;
    }
    public void GameClear()
    {
        if(gameState == Define.GameState.Cleared)
            return;

        gameState = Define.GameState.Cleared;
    }

    public void GameOver()
    {
        if(gameState == Define.GameState.GameOver) 
            return;
        
        gameState = Define.GameState.GameOver;
    }
}
