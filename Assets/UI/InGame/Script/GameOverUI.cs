using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backTitleButton;
    [SerializeField] private TextMeshProUGUI deadReasonText;

    private GameSession gameSession;

    private void Awake()
    {
        restartButton.onClick.AddListener(Restart);
        backTitleButton.onClick.AddListener(BackTitle);
    }

    private void Start()
    {
        gameSession = GameSession.Instance;

        if (gameSession == null)
        {
            Debug.LogError("GameOverUI could not find GameSession.", this);
            return;
        }

        gameSession.StateChanged += OnStateChanged;
        panel.SetActive(false);
    }

    public void OnDeadUI(string reason)
    {
        Time.timeScale = 0f;
        deadReasonText.text = reason;
        panel.SetActive(true);
    }

    private void OnStateChanged(GameSessionState state)
    {
        if (state == GameSessionState.GameOver)
            OnDeadUI(gameSession.GameOverReason);
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MapLoading");
    }

    private void BackTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    private void OnDestroy()
    {
        if (gameSession != null)
            gameSession.StateChanged -= OnStateChanged;
    }
}
