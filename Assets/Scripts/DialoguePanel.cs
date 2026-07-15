using TMPro;
using UnityEngine;

public sealed class DialoguePanel : MonoBehaviour
{
    public static DialoguePanel Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text dialogueText;

    private string speakerName;
    private string[] lines;
    private int lineIndex;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate DialoguePanel instance detected.", this);
            Destroy(this);
            return;
        }

        Instance = this;
        panel.SetActive(false);
    }

    public bool TryOpen(string speaker, string[] dialogueLines)
    {
        if (IsOpen || dialogueLines == null || dialogueLines.Length == 0)
            return false;

        speakerName = speaker;
        lines = dialogueLines;
        lineIndex = 0;
        IsOpen = true;
        panel.SetActive(true);
        RenderLine();
        return true;
    }

    public bool Advance()
    {
        if (!IsOpen)
            return false;

        lineIndex++;
        if (lineIndex < lines.Length)
        {
            RenderLine();
            return false;
        }

        IsOpen = false;
        panel.SetActive(false);
        return true;
    }

    private void RenderLine()
    {
        dialogueText.text = $"<b>{speakerName}</b>\n{lines[lineIndex]}";
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
