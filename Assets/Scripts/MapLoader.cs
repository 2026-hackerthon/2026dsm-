using UnityEngine;
using System.Collections.Generic;
public class MapLoader : MonoBehaviour
{
    public List<GameObject> Walls { get; private set; } = new();
    public List<GameObject> Objects { get; private set; } = new();
    public List<GameObject> MapTriggers { get; private set; } = new();
    public List<GameObject> PlayerLocations { get; private set; } = new();
    public GameObject Background { get; private set; }

    public void LoadMap(Transform mapRoot)
    {
        Walls.Clear();
        Objects.Clear();
        MapTriggers.Clear();
        PlayerLocations.Clear();
        Background = null;

        foreach (Transform child in mapRoot)
        {
            var type = MapObjectClassifier.GetType(child.name);

            switch (type)
            {
                case MapObjectType.Wall:
                    Walls.Add(child.gameObject);
                    SetInvisible(child.gameObject);
                    break;
                case MapObjectType.MapTrigger:
                    MapTriggers.Add(child.gameObject);
                    SetInvisible(child.gameObject);
                    break;
                case MapObjectType.PlayerLocation:
                    PlayerLocations.Add(child.gameObject);
                    SetInvisible(child.gameObject);
                    break;
                case MapObjectType.Object:
                    Objects.Add(child.gameObject);
                    break;
                case MapObjectType.Background:
                    Background = child.gameObject;
                    break;
            }
        }
    }

    // 콜라이더/트리거 기능은 유지하고 렌더러만 끔
    private void SetInvisible(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
            r.enabled = false;
    }

    public Transform GetSpawnPoint(string locationName = null)
{
    if (string.IsNullOrEmpty(locationName))
        return PlayerLocations.Count > 0 ? PlayerLocations[0].transform : null;

    var found = PlayerLocations.Find(p => p.name == locationName);
    return found != null ? found.transform
         : (PlayerLocations.Count > 0 ? PlayerLocations[0].transform : null);
}
}