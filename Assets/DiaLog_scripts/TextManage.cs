using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class TextManage : MonoBehaviour
{
    public static TextManage Instance { get; private set; }

    [SerializeField] private TalkManage talkData;
    [SerializeField] private TMP_Text talkText;
    [SerializeField] private GameObject talkPanel;

    public static TextManage GetOrFind()
    {
        return Instance != null ? Instance : FindAnyObjectByType<TextManage>(FindObjectsInactive.Include);
    }

    private void Awake()
    {
        Instance = this;
        EnsureEventSystem();
        talkPanel.SetActive(false);
    }

    private static void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            return;

        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        eventSystem.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
    }

    public bool TryOpen(string speaker, string[] lines)
    {
        if (!talkData.Begin(speaker, lines))
            return false;

        talkText.text = talkData.CurrentLine;
        talkPanel.SetActive(true);
        return true;
    }

    public bool Advance()
    {
        if (!talkData.MoveNext())
        {
            talkText.text = talkData.CurrentLine;
            return false;
        }

        talkPanel.SetActive(false);
        return true;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
