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

    private void Awake()
    {
        if (panel == null)
            panel = gameObject;

        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);

        if (backTitleButton != null)
            backTitleButton.onClick.AddListener(BackTitle);
    }

    public void OnDeadUI(string reason)
    {
        Time.timeScale = 0f;
        if (panel == null)
            panel = gameObject;

        if (deadReasonText != null)
            deadReasonText.text = reason;

        panel.SetActive(true);
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

}
