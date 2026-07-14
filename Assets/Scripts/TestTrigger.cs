using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    public GameObject targetDoor;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || targetDoor == null) return;
        MapTrigger trigger = targetDoor.GetComponent<MapTrigger>();
        trigger.isLocked = false;
    }
}