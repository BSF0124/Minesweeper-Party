using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject modePanel;
    [SerializeField] private GameObject difficultyPanel;

    [Header("Mode Button")]
    [SerializeField] private Button singleButton;
    [SerializeField] private Button coopButton;
    [SerializeField] private Button versusButton;

    [Header("Difficulty Button")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button cancelButton;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game";

    private void OnEnable()
    {
        singleButton.onClick.AddListener(HandleSingleClicked);
        coopButton.onClick.AddListener(HandleCoopClicked);
        versusButton.onClick.AddListener(HandleVersusClicked);

        easyButton.onClick.AddListener(HandleEasyClicked);
        normalButton.onClick.AddListener(HandleNormalClicked);
        hardButton.onClick.AddListener(HandleHardClicked);
        cancelButton.onClick.AddListener(HandleCancelClicked);
    }

    private void OnDisable()
    {
        singleButton.onClick.RemoveListener(HandleSingleClicked);
        coopButton.onClick.RemoveListener(HandleCoopClicked);
        versusButton.onClick.RemoveListener(HandleVersusClicked);

        easyButton.onClick.RemoveListener(HandleEasyClicked);
        normalButton.onClick.RemoveListener(HandleNormalClicked);
        hardButton.onClick.RemoveListener(HandleHardClicked);
        cancelButton.onClick.RemoveListener(HandleCancelClicked);
    }

    private void HandleSingleClicked()
    {
        modePanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    private void HandleCoopClicked()
    {
        
    }

    private void HandleVersusClicked()
    {
        
    }

    private void HandleEasyClicked()
    {
        GameStartContext.SetSingle(Define.Difficulty.Easy);
        SceneManager.LoadScene(gameSceneName);
    }

    private void HandleNormalClicked()
    {
        GameStartContext.SetSingle(Define.Difficulty.Normal);
        SceneManager.LoadScene(gameSceneName);
    }

    private void HandleHardClicked()
    {
        GameStartContext.SetSingle(Define.Difficulty.Hard);
        SceneManager.LoadScene(gameSceneName);
    }

    private void HandleCancelClicked()
    {
        difficultyPanel.SetActive(false);
        modePanel.SetActive(true);
    }
}
