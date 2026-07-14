using UnityEngine;
using TMPro;
public class TestDisplay : MonoBehaviour
{
    public GameObject targetDoor;
    public TextMeshPro displayText;  
    void Update()
    {
        if (targetDoor == null) return;
        MapTrigger trigger = targetDoor.GetComponent<MapTrigger>();
        displayText.text = "isLocked: " + trigger.isLocked.ToString();

    }
}