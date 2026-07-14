using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManage : MonoBehaviour
{
    public TalkManage TalkData;
    public TMP_Text TalkText;
    public GameObject ScanObject;
    public GameObject TalkPanel;
    public bool IsAction;
    public int TalkIndex;
    void Awake()
    {
        IsAction = false;
        TalkPanel.SetActive(false);
    }
    public void Action(GameObject scanObj)
    {
        
        ScanObject =  scanObj; 
        ObjData objData = ScanObject.GetComponent<ObjData>(); 
        Talk(objData.id, objData.IsNpc);
            
        TalkPanel.SetActive(IsAction);
    }

    void Talk(int id, bool IsNpc)
    {
        string talk = TalkData.GetTalk(id, TalkIndex);   // 변수명 talk로 변경
        if (talk == null)
        {
            IsAction = false;
            TalkIndex = 0;
            return;
        }
        if (IsNpc)
        {
            TalkText.text = talk;
        }
        else
        {
            TalkText.text = talk;
        }
        IsAction = true;
        TalkIndex++;
    }
}
