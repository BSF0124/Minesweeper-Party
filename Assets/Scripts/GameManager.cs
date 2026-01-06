using UnityEngine;

/* GameManager
 * 게임 전체 상태를 관리하는 전역 매니저
 * - Playing / GameOver / Cleared 상태 관리
 */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }    // 싱글톤 인스턴스
    public Define.GameState gameState { get; private set; }     // 현재 게임 상태

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

    // 게임 클리어 상태로 전환
    public void GameClear()
    {
        if(gameState == Define.GameState.Cleared)
            return;

        gameState = Define.GameState.Cleared;
    }

    // 게임 오버 상태로 전환
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
