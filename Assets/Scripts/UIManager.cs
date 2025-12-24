using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoardManager boardManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI remainingMineCountText;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        boardManager.OnRemainingMineCountChanged += UpdateRemainingMineText;
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(GameManager.Instance.RestartGame);
        boardManager.OnRemainingMineCountChanged -= UpdateRemainingMineText;
    }

    private void Update()
    {
        timerText.text = FormatTime(GameManager.Instance.elapsedTime);
    }

    private string FormatTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        string formatted = time.ToString(@"mm\:ss\.ff");
        return formatted;
    }

    private void UpdateRemainingMineText(int remaining)
    {
        remainingMineCountText.text = remaining.ToString("D3");
    }
}
