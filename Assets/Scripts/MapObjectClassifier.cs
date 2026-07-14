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

        return MapObjectType.Object;
    }
}
