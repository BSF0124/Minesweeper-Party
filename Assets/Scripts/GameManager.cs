using UnityEngine;

/* GameManager
 * 게임 전체의 진행 상태를 관리하는 전역 매니저
 * - Playing / GameOver / Cleared 상태 관리
 * 게임 규칙 판단은 수행하지 않음
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

    // 타이머 시작
    public void StartTimer()
    {
        if(!isTimerRunning)
            isTimerRunning = true;
    }

    // 타이머 초기화
    public void ResetTimer()
    {
        isTimerRunning = false;
        elapsedTime = 0f;
        gameState = Define.GameState.Playing;
    }
}
