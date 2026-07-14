using System.Collections.Generic;
using UnityEngine;

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
            switch (MapObjectClassifier.GetType(child.name))
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

    private void SetInvisible(GameObject obj)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
            renderer.enabled = false;
    }

    public Transform GetSpawnPoint(string locationName)
    {
        if (string.IsNullOrWhiteSpace(locationName))
        {
            Debug.LogError($"Map '{gameObject.name}' received an empty spawn point name.", this);
            return null;
        }

        var found = PlayerLocations.Find(location => location != null && location.name == locationName);
        if (found == null)
            Debug.LogError($"Map '{gameObject.name}' does not contain spawn point '{locationName}'.", this);

        return found != null ? found.transform : null;
    }
}
