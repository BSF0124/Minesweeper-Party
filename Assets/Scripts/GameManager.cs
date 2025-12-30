using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Define.GameState gameState { get; private set; }

    private bool isTimerRunning = false;
    public float elapsedTime = 0f;

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

    private void Update()
    {
        if(isTimerRunning && gameState == Define.GameState.Playing)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    // 모든 안전 셀이 열렸을 때 호출
    // 게임 상태를 Cleared로 전환
    public void GameClear()
    {
        if(gameState == Define.GameState.Cleared)
            return;

        gameState = Define.GameState.Cleared;
    }

    // 지뢰 클릭 시 호출
    // 게임 상태를 GameOver로 전환
    public void GameOver()
    {
        if(gameState == Define.GameState.GameOver) 
            return;
        
        gameState = Define.GameState.GameOver;
    }

    public void StartTimer()
    {
        if(!isTimerRunning)
            isTimerRunning = true;
    }

    public void ResetTimer()
    {
        isTimerRunning = false;
        elapsedTime = 0f;
        gameState = Define.GameState.Playing;
    }
}
