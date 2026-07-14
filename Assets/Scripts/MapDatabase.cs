using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDatabase", menuName = "MapSystem/MapDatabase")]
public class MapDatabase : ScriptableObject
{
    public List<MapDataEntry> maps;

    public MapDataEntry GetMap(string mapName)
    {
        if (maps == null)
        {
            Debug.LogError("MapDatabase maps list is not assigned.", this);
            return null;
        }

        if (string.IsNullOrWhiteSpace(mapName))
        {
            Debug.LogError("MapDatabase received an empty map name.", this);
            return null;
        }

        int matchingCount = 0;
        foreach (var entry in maps)
        {
            if (entry == null)
            {
                Debug.LogError("MapDatabase contains a missing MapDataEntry.", this);
                return null;
            }

            if (entry.mapName == mapName)
                matchingCount++;
        }

        if (matchingCount > 1)
        {
            Debug.LogError($"MapDatabase contains duplicate map name '{mapName}'.", this);
            return null;
        }

        var map = maps.Find(entry => entry.mapName == mapName);
        if (map == null)
            Debug.LogError($"MapDatabase could not find map '{mapName}'.", this);

        return map;
    }
}
