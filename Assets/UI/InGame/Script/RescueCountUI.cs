using TMPro;
using UnityEngine;

public class RescueCountUI : MonoBehaviour
{
    private TMP_Text rescueCountText;
    private GameSession gameSession;

    private void Awake()
    {
        rescueCountText = GetComponent<TMP_Text>();

        if (rescueCountText == null)
        {
            Debug.LogError("RescueCountUI requires a TMP text component.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        gameSession = GameSession.Instance;

        if (gameSession == null)
        {
            Debug.LogError("RescueCountUI could not find GameSession.", this);
            enabled = false;
            return;
        }

        gameSession.RescueCountChanged += UpdateCount;
        UpdateCount(gameSession.RescuedStudentCount);
    }

    private void UpdateCount(int count)
    {
        rescueCountText.text = $"{count}/{GameSession.MaxRescuedStudents}";
    }

    private void OnDestroy()
    {
        if (gameSession != null)
            gameSession.RescueCountChanged -= UpdateCount;
    }
}
