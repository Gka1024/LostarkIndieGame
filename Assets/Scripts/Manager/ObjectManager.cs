using System.Collections.Generic;
using Unity.Mathematics;
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
    public GameObject obstacleRightUp;

    // ==== PatternF_Create_Pillars

    public List<HexTile> imposterTiles;
    public GameObject pillarObject;


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
        tileObjects[TileSpecific.ObstacleRightUp] = obstacleRightUp;
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

    public HexTile IsObjectExist(List<HexTile> tiles, TileState state)
    {
        foreach (HexTile tile in tiles)
        {
            if (tile.currentTileState == state)
            {
                return tile;
            }
        }
        return null;
    }

    public void DestroyObjectByTile(HexTile tile)
    {
        if (tile.currentTileSpecific == TileSpecific.Default) return;

        tileObjects.TryGetValue(tile.currentTileSpecific, out GameObject obj);
        Destroy(obj);

        tileGroups.TryGetValue(tile.currentTileSpecific, out List<HexTile> tiles);

        foreach (HexTile objectTile in tiles)
        {
            objectTile.currentTileSpecific = TileSpecific.Default;
        }
    }

    public void DestroyObject(GameObject obj)
    {

    }

    public void BreakAllWalls()
    {
        List<TileSpecific> wallTypes = new List<TileSpecific>()
    {
        TileSpecific.WallLeftUp,
        TileSpecific.WallLeftDown,
        TileSpecific.WallRightUp,
        TileSpecific.WallRightDown,
        TileSpecific.WallFront
    };

        foreach (var type in wallTypes)
        {
            // 1️⃣ 게임 오브젝트 제거
            if (tileObjects.TryGetValue(type, out GameObject obj) && obj != null)
            {
                Destroy(obj);
            }

            // 2️⃣ 타일 상태 초기화
            if (tileGroups.TryGetValue(type, out List<HexTile> tiles))
            {
                foreach (HexTile tile in tiles)
                {
                    tile.currentTileState = TileState.Default;
                    tile.currentTileSpecific = TileSpecific.Default;
                }

                tiles.Clear();
            }
        }
    }

    public void BreakAllPillars()
    {
        List<TileSpecific> pillarTypes = new List<TileSpecific>()
    {
        TileSpecific.PillarLeftUp,
        TileSpecific.PillarLeftMiddle,
        TileSpecific.PillarLeftDown,
        TileSpecific.PillarRightUp,
        TileSpecific.PillarRightMiddle,
        TileSpecific.PillarRightDown
    };

        foreach (var type in pillarTypes)
        {
            // 1️⃣ 게임 오브젝트 제거
            if (tileObjects.TryGetValue(type, out GameObject obj) && obj != null)
            {
                Destroy(obj);
            }

            // 2️⃣ 타일 상태 초기화
            if (tileGroups.TryGetValue(type, out List<HexTile> tiles))
            {
                foreach (HexTile tile in tiles)
                {
                    tile.currentTileState = TileState.Default;
                    tile.currentTileSpecific = TileSpecific.Default;
                }

                tiles.Clear();
            }
        }
    }

    public List<HexTile> GetTiles(TileSpecific type)
    {
        if (tileGroups.TryGetValue(type, out var list))
            return list;

        return null;
    }

    // ================== 

    public void CreatePillarForImposter()
    {
        foreach (HexTile tile in imposterTiles)
        {
            tile.currentTileSpecific = TileSpecific.PillarForPattern;
            tile.currentTileState = TileState.IsPillar;
            Vector3 ObjectPos = new Vector3(tile.transform.position.x, 1.5f, tile.transform.position.z);

            Instantiate(pillarObject, ObjectPos, quaternion.identity);
        }
    }

    
}
