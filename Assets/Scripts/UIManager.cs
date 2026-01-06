using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* UIManager
 * 인게임 화면에서 표시되는 UI 요소를 관리하는 클래스
 * - 게임 경과 시간 표시
 * - 남은 지뢰 개수 표시
 * - 재시작 버튼 입력 처리
 */
public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoardManager boardManager;
    // 보드 리셋 및 지뢰 개수 이벤트 구독을 위한 참조

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    // 경과 시간 표시 텍스트
    [SerializeField] private TextMeshProUGUI remainingMineCountText;
    // 남은 지뢰 개수 표시 텍스트
    [SerializeField] private Button restartButton;
    // 보드 재시작 버튼

    // UI 이벤트 및 보드 이벤트 구독
    private void OnEnable()
    {
        restartButton.onClick.AddListener(boardManager.ResetBoard);
        boardManager.OnRemainingMineCountChanged += UpdateRemainingMineText;
    }

    //  구독한 이벤트 해제
    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(boardManager.ResetBoard);
        boardManager.OnRemainingMineCountChanged -= UpdateRemainingMineText;
    }

    // 매 프레임마다 GameManager로부터
    // 현재 경과 시간을 받아 타이머 UI를 갱신
    private void Update()
    {
        timerText.text = FormatTime(GameManager.Instance.elapsedTime);
    }

    // 초 단위 시간을 mm:ss.ff 형식 문자열로 변환
    private string FormatTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        string formatted = time.ToString(@"mm\:ss\.ff");
        return formatted;
    }

    // BoardManager에서 전달받은 남은 지뢰 수를
    // 3자리 숫자 형식으로 UI에 반영
    private void UpdateRemainingMineText(int remaining)
    {
        remainingMineCountText.text = remaining.ToString("D3");
    }
}
