using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ClearUI : MonoBehaviour
{
    private GameSession gameSession;
    private GameObject panel;

    private void Start()
    {
        gameSession = GameSession.Instance;
        if (gameSession != null)
            gameSession.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameSessionState state)
    {
        if (state != GameSessionState.Cleared)
            return;

        if (panel == null)
            panel = CreatePanel();

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    private GameObject CreatePanel()
    {
        GameObject overlay = new GameObject("ClearPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        overlay.transform.SetParent(transform, false);
        overlay.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.72f);

        RectTransform overlayRect = overlay.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;

        GameObject label = new GameObject("ClearText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        label.transform.SetParent(overlay.transform, false);

        TextMeshProUGUI text = label.GetComponent<TextMeshProUGUI>();
        text.font = TMP_Settings.defaultFontAsset;
        text.fontSize = 48f;
        text.alignment = TextAlignmentOptions.Center;
        text.text = $"ESCAPED!\nStudents rescued: {gameSession.RescuedStudentCount}/{GameSession.MaxRescuedStudents}";

        RectTransform textRect = label.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(800f, 220f);
        return overlay;
    }

    private void OnDestroy()
    {
        if (gameSession != null)
            gameSession.StateChanged -= OnStateChanged;
    }
}
