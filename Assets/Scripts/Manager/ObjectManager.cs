using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<TileSpecific, List<HexTile>> tileGroups = new();
    private Dictionary<TileSpecific, GameObject> tileObjects = new();
    private Dictionary<HexTile, GameObject> tileObjectMap = new();

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

    public GameObject InnerWalls;
    public GameObject OuterWalls;
    public GameObject OuterWallsDown;
    public GameObject OuterWallsUp;
    public GameObject OuterWallsMiddle;

    [Header("Obstacles")]
    public GameObject obstacleLeftUp;
    public GameObject obstacleRightUp;

    // ==== PatternF_Create_Pillars

    public GameObject pillarObject;


    private void Start()
    {
        RegisterTileObjects();
    }

    private void RegisterTileObjects()
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
        if (tile.currentTileState == TileState.Default) return;

        var type = tile.currentTileSpecific;

        if (!tileGroups.ContainsKey(type))
        {
            tileGroups[type] = new List<HexTile>();
        }

        tileGroups[type].Add(tile);
    }

    public void RegisterObject(ObstaclesScript obj)
    {

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

    public void DestroyObjectByTile(HexTile tile) // 여러개에 걸쳐 있는 오브젝트를 제거하기 위해 사용
    {
        if (tile.currentTileState == TileState.Default) return;

        if (tileObjects.TryGetValue(tile.currentTileSpecific, out GameObject obj))
        {
            obj.GetComponent<ObstaclesScript>().DestroyObject();

            foreach (HexTile tileToChange in tileGroups.GetValueOrDefault(tile.currentTileSpecific))
            {
                tileToChange.SetTileState(TileState.Default);
            }
        }
    }

    public void DestroyObjectBySpecificTile(HexTile tile) // 하나의 타일에 있는 오브젝트 제거하기 위해 사용
    {
        if (tile.currentTileState == TileState.Default) return;

        if (tileObjectMap.TryGetValue(tile, out GameObject obj))
        {
            Destroy(obj);
            tileObjectMap.Remove(tile);
        }

        tile.SetTileState(TileState.Default);
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

        HexTileManager.Instance.ResetAllTileState();
    }

    public List<HexTile> GetTiles(TileSpecific type)
    {
        if (tileGroups.TryGetValue(type, out var list))
            return list;

        return null;
    }

    // ================== 

    public void CreatePillarForImposter(List<HexTile> imposterTiles)
    {
        foreach (HexTile tile in imposterTiles)
        {
            tile.currentTileSpecific = TileSpecific.PillarForPattern;
            tile.currentTileState = TileState.IsPillar;
            Vector3 ObjectPos = new Vector3(tile.transform.position.x, 1.5f, tile.transform.position.z);

            Register(tile);

            GameObject obj = Instantiate(pillarObject, ObjectPos, quaternion.identity);

            if (Player.Instance.move.GetCurrentTile() == tile)
            {
                int randomTileindex = UnityEngine.Random.Range(0, tile.neighbors.Count());
                Player.Instance.move.MoveToTile(new PlayerMoveInfo(tile.neighbors[randomTileindex]));
            }

            tileObjectMap[tile] = obj;
        }
    }

    public void DestroyOuterWallsDown()
    {
        OuterWallsDown.SetActive(false);
    }

    public void DestroyOuterWallsUP()
    {
        OuterWallsUp.SetActive(false);
    }

    public void DestroyOuterWallsMiddle()
    {
        OuterWallsMiddle.SetActive(false);
    }


    public void DestroyInnerWalls()
    {
        InnerWalls.SetActive(false);
    }
}
