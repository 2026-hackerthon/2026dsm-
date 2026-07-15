using TMPro;
using UnityEngine;

public class TextManage : MonoBehaviour
{
    public TalkManage TalkData;
    public TMP_Text TalkText;
    public GameObject ScanObject;
    public GameObject TalkPanel;
    public bool IsAction;
    public int TalkIndex;

    private void Awake()
    {
        IsAction = false;
        TalkPanel.SetActive(false);
    }

    public void Action(GameObject scanObj)
    {
        ScanObject = scanObj;
        ObjData objData = ScanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.IsNpc);
        TalkPanel.SetActive(IsAction);
    }

    private void Talk(int id, bool isNpc)
    {
        string talk = TalkData.GetTalk(id, TalkIndex);
        if (talk == null)
        {
            IsAction = false;
            TalkIndex = 0;
            return;
        }

        TalkText.text = talk;
        IsAction = true;
        TalkIndex++;
    }
}
