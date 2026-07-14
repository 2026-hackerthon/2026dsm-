using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("Map Database")]
    public MapDatabase mapDatabase;

    [Header("Start Map")]
    public MapDataEntry startMap;
    public string startSpawnLocationName;

    [Header("Camera Placement")]
    public Camera mainCamera;
    public float mapZOffset = 10f;

    private GameObject currentMapInstance;
    private MapLoader currentMapLoader;

    public Transform mapParent;
    public GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate MapManager instance detected.", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        if (startMap != null)
            LoadMap(startMap, startSpawnLocationName);
    }

    public void LoadMap(string mapName, string spawnLocationName = null)
    {
        if (mapDatabase == null)
        {
            Debug.LogError("MapManager has no MapDatabase assigned.", this);
            return;
        }

        LoadMap(mapDatabase.GetMap(mapName), spawnLocationName);
    }

    public void LoadMap(MapDataEntry entry, string spawnLocationName = null)
    {
        if (mapDatabase == null)
        {
            Debug.LogError("MapManager has no MapDatabase assigned.", this);
            return;
        }

        if (entry == null)
        {
            Debug.LogError("MapManager received no MapDataEntry.", this);
            return;
        }

        if (entry.prefab == null)
        {
            Debug.LogError($"Map '{entry.mapName}' has no prefab assigned.", entry);
            return;
        }

        if (entry.prefab.GetComponent<MapLoader>() == null)
        {
            Debug.LogError($"Map prefab '{entry.prefab.name}' has no MapLoader component.", entry.prefab);
            return;
        }

        var newMapInstance = Instantiate(entry.prefab, mapParent);
        newMapInstance.transform.localPosition = Vector3.zero;
        var newMapLoader = newMapInstance.GetComponent<MapLoader>();
        newMapLoader.LoadMap(newMapInstance.transform);

        var spawnPoint = newMapLoader.GetSpawnPoint(spawnLocationName);
        if (spawnPoint == null)
        {
            Destroy(newMapInstance);
            return;
        }

        if (player == null)
        {
            Debug.LogError("MapManager has no player assigned.", this);
            Destroy(newMapInstance);
            return;
        }

        PositionMapAtCamera(newMapInstance);

        if (currentMapInstance != null)
            Destroy(currentMapInstance);

        player.transform.position = spawnPoint.position;
        currentMapInstance = newMapInstance;
        currentMapLoader = newMapLoader;
    }

    private void PositionMapAtCamera(GameObject mapInstance)
    {
        if (mainCamera == null || mapParent == null)
            return;

        var cameraPosition = mainCamera.transform.position;
        mapParent.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + mapZOffset);

        var renderers = mapInstance.GetComponentsInChildren<Renderer>();
        var hasBounds = false;
        var mapBounds = new Bounds();

        foreach (var renderer in renderers)
        {
            if (!renderer.enabled)
                continue;

            if (!hasBounds)
            {
                mapBounds = renderer.bounds;
                hasBounds = true;
            }
            else
            {
                mapBounds.Encapsulate(renderer.bounds);
            }
        }

        if (!hasBounds)
        {
            Debug.LogError($"Map '{mapInstance.name}' has no visible renderer to center.", mapInstance);
            return;
        }

        var offset = cameraPosition - mapBounds.center;
        mapInstance.transform.position += new Vector3(offset.x, offset.y, 0f);
    }
}
