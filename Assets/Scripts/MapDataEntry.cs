using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "MapSystem/MapDataEntry")]
public class MapDataEntry : ScriptableObject
{
    public string mapName;
    public GameObject prefab;
}
