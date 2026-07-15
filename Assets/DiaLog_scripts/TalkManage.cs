using UnityEngine;

public class TalkManage : MonoBehaviour
{
    private string speaker;
    private string[] lines;
    private int index;

    public bool Begin(string speakerName, string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return false;

        speaker = speakerName;
        lines = dialogueLines;
        index = 0;
        return true;
    }

    public string CurrentLine => $"<b>{speaker}</b>\n{lines[index]}";

    public bool MoveNext()
    {
        index++;
        return index >= lines.Length;
    }
}
