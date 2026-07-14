using UnityEngine;
using System.Collections.Generic;

public enum MapObjectType
{
    Background,
    Wall,
    Object,
    MapTrigger,
    PlayerLocation
}

public static class MapObjectClassifier
{
    public static MapObjectType GetType(string objectName)
    {
        if (objectName.StartsWith("Wall")) return MapObjectType.Wall;
        if (objectName.StartsWith("MapTrigger")) return MapObjectType.MapTrigger;
        if (objectName.StartsWith("PlayerLocation")) return MapObjectType.PlayerLocation;
        if (objectName.StartsWith("Background")) return MapObjectType.Background;

        return MapObjectType.Object; // 나머지는 다 Object로 취급
    }
}

[CreateAssetMenu(fileName = "MapData", menuName = "MapSystem/MapDataEntry")]
public class MapDataEntry : ScriptableObject
{
    public string mapName;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "MapDatabase", menuName = "MapSystem/MapDatabase")]
public class MapDatabase : ScriptableObject
{
    public List<MapDataEntry> maps;

    public MapDataEntry GetMap(string mapName)
    {
        return maps.Find(m => m.mapName == mapName);
    }
}