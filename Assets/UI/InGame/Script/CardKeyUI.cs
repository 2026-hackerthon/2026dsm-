using UnityEngine;

public class CardKeyUI : MonoBehaviour
{
    [SerializeField] private GameObject icon;

    private GameSession gameSession;

    private void Start()
    {
        if (icon == null)
        {
            Debug.LogError("CardKeyUI icon is not assigned.", this);
            enabled = false;
            return;
        }

        gameSession = GameSession.Instance;

        if (gameSession == null)
        {
            Debug.LogError("CardKeyUI could not find GameSession.", this);
            enabled = false;
            return;
        }

        gameSession.CardKeyChanged += UpdateIcon;
        UpdateIcon(gameSession.HasCardKey);
    }

    private void UpdateIcon(bool hasCardKey)
    {
        icon.SetActive(hasCardKey);
    }

    private void OnDestroy()
    {
        if (gameSession != null)
            gameSession.CardKeyChanged -= UpdateIcon;
    }
}
