using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<TileSpecific, List<HexTile>> tileGroups = new();
    private Dictionary<TileSpecific, GameObject> tileObjects = new();

    [Header("Pillars")]
    public GameObject pillarLeftUp;
    public GameObject pillarLeftMiddle;
    public GameObject pillarLeftDown;
    public GameObject pillarRightUp;
    public GameObject pillarRightMiddle;
    public GameObject pillarRightDown;

    [Header("Walls")]
    public GameObject wallLeftUp;
    public GameObject wallLeftDown;
    public GameObject wallRightUp;
    public GameObject wallRightDown;
    public GameObject wallFront;

    [Header("Obstacles")]
    public GameObject obstacleLeftUp;
    public GameObject obstacleLeftDown;
    public GameObject obstacleRightUp;
    public GameObject obstacleRightDown;

    private void Awake()
    {
        tileObjects.Clear();

        tileObjects[TileSpecific.PillarLeftUp] = pillarLeftUp;
        tileObjects[TileSpecific.PillarLeftMiddle] = pillarLeftMiddle;
        tileObjects[TileSpecific.PillarLeftDown] = pillarLeftDown;

        tileObjects[TileSpecific.PillarRightUp] = pillarRightUp;
        tileObjects[TileSpecific.PillarRightMiddle] = pillarRightMiddle;
        tileObjects[TileSpecific.PillarRightDown] = pillarRightDown;

        tileObjects[TileSpecific.WallLeftUp] = wallLeftUp;
        tileObjects[TileSpecific.WallLeftDown] = wallLeftDown;
        tileObjects[TileSpecific.WallRightUp] = wallRightUp;
        tileObjects[TileSpecific.WallRightDown] = wallRightDown;
        tileObjects[TileSpecific.WallFront] = wallFront;

        tileObjects[TileSpecific.ObstacleLeftUp] = obstacleLeftUp;
        tileObjects[TileSpecific.ObstacleLeftDown] = obstacleLeftDown;
        tileObjects[TileSpecific.ObstacleRightUp] = obstacleRightUp;
        tileObjects[TileSpecific.ObstacleRightDown] = obstacleRightDown;
    }

    public void Register(HexTile tile)
    {
        if (tile.currentTileSpecific == TileSpecific.Default) return;

        var type = tile.currentTileSpecific;

        if (!tileGroups.ContainsKey(type))
        {
            tileGroups[type] = new List<HexTile>();
        }

        tileGroups[type].Add(tile);
    }

    public void DestroyObjectByTile(HexTile tile)
    {
        if (tile.currentTileSpecific == TileSpecific.Default) return;

        tileObjects.TryGetValue(tile.currentTileSpecific, out GameObject obj);

        Destroy(obj);
    }

    public List<HexTile> GetTiles(TileSpecific type)
    {
        if (tileGroups.TryGetValue(type, out var list))
            return list;

        return null;
    }
}
