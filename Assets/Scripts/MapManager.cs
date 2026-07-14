using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("맵 데이터베이스")]
    public MapDatabase mapDatabase;

    [Header("시작 맵 설정")]
    public MapDataEntry startMap;
    public string startSpawnLocationName;

    [Header("카메라 배치")]
    public Camera mainCamera;
    public float mapZOffset = 10f;

    private GameObject currentMapInstance;
    private MapLoader currentMapLoader;

    public Transform mapParent;
    public GameObject player;

    private void Awake()
    {
        Instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        if (startMap != null)
            LoadMap(startMap, startSpawnLocationName);
    }

    // 맵 이름(string)으로 로드 - MapTrigger 등에서 사용
    public void LoadMap(string mapName, string spawnLocationName = null)
    {
        var entry = mapDatabase.GetMap(mapName);
        if (entry == null)
        {
            Debug.LogError($"맵을 찾을 수 없음: {mapName}");
            return;
        }
        LoadMap(entry, spawnLocationName);
    }

    // MapDataEntry로 직접 로드 - 시작 맵 등 인스펙터에서 직접 참조할 때 사용
    public void LoadMap(MapDataEntry entry, string spawnLocationName = null)
    {
        if (currentMapInstance != null)
            Destroy(currentMapInstance);

        PositionMapParentAtCamera();

        currentMapInstance = Instantiate(entry.prefab, mapParent);
        currentMapInstance.transform.localPosition = Vector3.zero;
        currentMapLoader = currentMapInstance.GetComponent<MapLoader>();

        if (currentMapLoader == null)
        {
            Debug.LogError("맵 프리팹에 MapLoader가 없음");
            return;
        }

        currentMapLoader.LoadMap(currentMapInstance.transform);

        var spawnPoint = currentMapLoader.GetSpawnPoint(spawnLocationName);
        if (spawnPoint != null && player != null)
        {
            player.transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("스폰 위치를 찾지 못함");
        }
    }

    private void PositionMapParentAtCamera()
    {
        if (mainCamera == null || mapParent == null) return;

        Vector3 camPos = mainCamera.transform.position;
        mapParent.position = new Vector3(camPos.x, camPos.y,camPos.z  + mapZOffset);
    }
}