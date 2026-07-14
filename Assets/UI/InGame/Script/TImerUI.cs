using TMPro;
using UnityEngine;

public class TImerUI : MonoBehaviour
{
    [SerializeField] private Color warningColor = new Color(0.9f, 0.2f, 0.2f);

    private TMP_Text timerText;
    private Color normalColor;
    private GameSession gameSession;

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
        UpdateTime(gameSession.RemainingTime);
    }

    private void UpdateTime(float remainingTime)
    {
        int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));
        timerText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
        timerText.color = remainingTime <= 10f ? warningColor : normalColor;
    }

    private void OnDestroy()
    {
        if (gameSession != null)
            gameSession.RemainingTimeChanged -= UpdateTime;
    }
}
