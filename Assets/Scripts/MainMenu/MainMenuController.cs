using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* MainMenuController
 * 메인 메뉴 화면에서
 * - 게임 모드 선택
 * - 난이도 선택
 * - 게임 씬 진입
 * 을 제어하는 UI 컨트롤러
 */
public class MainMenuController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject modePanel;          // 게임 모드 선택 UI 패널
    [SerializeField] private GameObject difficultyPanel;    // 난이도 선택 UI 패널

    [Header("Mode Button")]
    [SerializeField] private Button classicButton;  // 클래식 모드 선택 버튼
    [SerializeField] private Button coopButton;     // 협동 모드 선택 버튼
    [SerializeField] private Button versusButton;   // 대전 모드 선택 버튼

    [Header("Difficulty Button")]
    [SerializeField] private Button easyButton;     // 쉬움 난이도 버튼
    [SerializeField] private Button normalButton;   // 보통 난이도 버튼
    [SerializeField] private Button hardButton;     // 어려움 난이도 버튼
    [SerializeField] private Button cancelButton;   // 난이도 선택 취소 버튼

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game"; // 게임 씬 이름

    // 각 UI 버튼에 클릭 이벤트를 등록
    private void OnEnable()
    {
        classicButton.onClick.AddListener(HandleClassicClicked);
        coopButton.onClick.AddListener(HandleCoopClicked);
        versusButton.onClick.AddListener(HandleVersusClicked);

        easyButton.onClick.AddListener(HandleEasyClicked);
        normalButton.onClick.AddListener(HandleNormalClicked);
        hardButton.onClick.AddListener(HandleHardClicked);
        cancelButton.onClick.AddListener(HandleCancelClicked);
    }

    // 등록된 UI 이벤트 해제
    private void OnDisable()
    {
        classicButton.onClick.RemoveListener(HandleClassicClicked);
        coopButton.onClick.RemoveListener(HandleCoopClicked);
        versusButton.onClick.RemoveListener(HandleVersusClicked);

        easyButton.onClick.RemoveListener(HandleEasyClicked);
        normalButton.onClick.RemoveListener(HandleNormalClicked);
        hardButton.onClick.RemoveListener(HandleHardClicked);
        cancelButton.onClick.RemoveListener(HandleCancelClicked);
    }

    // 클래식 모드 선택 시
    // 난이도 선택 패널로 전환
    private void HandleClassicClicked()
    {
        modePanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }


    // 협동 모드 버튼 클릭 처리
    private void HandleCoopClicked()
    {
        
    }

    // 대전 모드 버튼 클릭 처리
    private void HandleVersusClicked()
    {
        
    }

    // 쉬움 난이도로 클래식 게임 모드 시작
    private void HandleEasyClicked()
    {
        GameStartContext.SetClassic(Define.Difficulty.Easy);
        SceneManager.LoadScene(gameSceneName);
    }

    // 보통 난이도로 클래식 게임 모드 시작
    private void HandleNormalClicked()
    {
        GameStartContext.SetClassic(Define.Difficulty.Normal);
        SceneManager.LoadScene(gameSceneName);
    }

    // 어려움 난이도로 클래식 게임 모드 시작
    private void HandleHardClicked()
    {
        GameStartContext.SetClassic(Define.Difficulty.Hard);
        SceneManager.LoadScene(gameSceneName);
    }

    // 난이도 선택을 취소하고
    // 다시 게임 모드 선택 화면으로 돌아감
    private void HandleCancelClicked()
    {
        difficultyPanel.SetActive(false);
        modePanel.SetActive(true);
    }
}
