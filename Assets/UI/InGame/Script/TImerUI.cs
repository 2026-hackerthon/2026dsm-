using TMPro;
using UnityEngine;

public class TImerUI : MonoBehaviour
{
    [SerializeField] private Color warningColor = new Color(0.8f, 0, 0);

    private TMP_Text timerText;
    private Color normalColor;
    private GameSession gameSession;
    
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        timerText = GetComponent<TMP_Text>();

        if (timerText == null)
        {
            Debug.LogError("TImerUI requires a TMP text component.", this);
            enabled = false;
            return;
        }

        normalColor = timerText.color;
    }

    private void Start()
    {
        gameSession = GameSession.Instance;

        if (gameSession == null)
        {
            Debug.LogError("TImerUI could not find GameSession.", this);
            enabled = false;
            return;
        }

        gameSession.RemainingTimeChanged += UpdateTime;
        gameSession.StateChanged += OnGameOver;
        UpdateTime(gameSession.RemainingTime);
        OnGameOver(gameSession.State);
    }

    private void UpdateTime(float remainingTime)
    {
        float totalSeconds = Mathf.Max(0f, remainingTime);
        timerText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00.00}";
        timerText.color = remainingTime <= 10f ? warningColor : normalColor;
        
    }

    private void OnGameOver(GameSessionState gameSessionState)
    {
        if (gameSessionState == GameSessionState.GameOver)
        {
            if (gameOverUI == null)
            {
                Debug.LogError("TImerUI requires a GameOverUI reference.", this);
                return;
            }

            gameOverUI.OnDeadUI(gameSession.GameOverReason);
        }
    }

    private void OnDestroy()
    {
        if (gameSession != null)
        {
            gameSession.RemainingTimeChanged -= UpdateTime;
            gameSession.StateChanged -= OnGameOver;
        }
    }
}
