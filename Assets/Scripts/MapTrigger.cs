using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    public string targetMapName;
    public string targetPlayerLocationName;
    public bool isLocked;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || this.isLocked == true) return;
        MapManager.Instance.LoadMap(targetMapName, targetPlayerLocationName);
    }
}