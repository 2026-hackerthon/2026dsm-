using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    public string targetMapName;
    public string targetPlayerLocationName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        MapManager.Instance.LoadMap(targetMapName, targetPlayerLocationName);
    }
}