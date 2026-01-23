using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class HexTileManager : MonoBehaviour
{
    public static HexTileManager Instance { get; private set; }

    public HexTileSelectHandler HexTileSelectHandler;
    public TileDirectionHelper tileDirectionHelper;

    public TileDistanceHelper tileDistanceHelper;
    public TileRayHelper tileRayHelper;
    public TileBackHelper tileBackHelper;
    public TileFrontHelper tileFrontHelper;

    public GameObject boss;
    public HexTile[] allTiles;

    [SerializeField]
    private List<HexTile> playerMoveRange = new List<HexTile>();

    private Dictionary<Vector3Int, HexTile> tileMap = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }
        allTiles = FindObjectsByType<HexTile>(FindObjectsSortMode.None);
    }

    void Start()
    {
        FindNeighbor();
        RegisterTileCubeCoord();
    }

    private void FindNeighbor()
    {
        foreach (HexTile tile in allTiles)
        {
            tile.FindNeighbors(allTiles);
        }
    }

    public void RegisterTile(GameObject obj)
    {
        HexTileSelectHandler.GetComponent<HexTileSelectHandler>().RegisterHextile(obj);
    }

    // 타일의 속성으로 타일을 반환하는 코드들입니다. 

    public List<HexTile> GetAllTiles()
    {
        List<HexTile> resultTiles = new();

        foreach (HexTile tile in allTiles)
        {
            resultTiles.Add(tile);
        }

        return resultTiles;
    }

    public HexTile GetObjectHextile(GameObject obj)
    {
        Vector3 objPosition = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
        foreach (HexTile tile in allTiles)
        {
            Vector3 tilePosition = new Vector3(tile.transform.position.x, 0, tile.transform.position.z);

            if (Vector3.Distance(tilePosition, objPosition) < 0.2f)
            {
                return tile;
            }
        }
        return null;
    }

    public HexTile IsThereHexTile(Vector3 pos)
    {
        foreach (HexTile tile in allTiles)
        {
            if (Vector3.Distance(tile.transform.position, pos) < 0.5)
            {
                return tile;
            }
        }
        return null;
    }

    public HexTile IsThereHexTileByCube(Vector3Int pos)
    {
        if (tileMap.TryGetValue(pos, out HexTile tile))
        {
            return tile;
        }
        return null;
    }

    public List<HexTile> GetMoveableTiles()
    {
        List<HexTile> tileList = new List<HexTile>();
        foreach (HexTile tile in allTiles)
        {
            if (tile.GetIsPlayerMoveable())
            {
                tileList.Add(tile);
            }
        }
        return tileList;
    }

    public bool IsTileMoveable(HexTile startTile, HexTile targetTile, int moveAbleDistance)
    {
        if (startTile == targetTile || targetTile.GetIsTileMoveable() == false || IsBossTile(targetTile))
        {
            return false;
        }

        return GetTileDistance(startTile, targetTile) <= moveAbleDistance;
    }

    public List<HexTile> GetInvertedTiles(List<HexTile> tiles)
    {
        List<HexTile> alltiles = GetAllTiles();
        return alltiles.Except(tiles).ToList();
    }

    public HexTile GetRandomTile(List<HexTile> tiles)
    {
        int ranNum = Random.Range(0, tiles.Count);
        Debug.Log(ranNum);
        return tiles[ranNum];
    }

    public HexTile GetNearestTile(HexTile startTile, HexTile targetTile)
    {
        if (startTile.neighbors == null) return startTile;

        HexTile resultTile = startTile;
        Vector3 targetPos = targetTile.transform.position;
        Vector3 resultPos = resultTile.transform.position;

        float minDistanceSqr = (resultPos - targetPos).sqrMagnitude;

        foreach (HexTile tile in startTile.neighbors)
        {
            Vector3 tilePos = tile.transform.position;
            float distanceSqr = (tilePos - targetPos).sqrMagnitude;

            if (distanceSqr < minDistanceSqr)
            {
                resultTile = tile;
                minDistanceSqr = distanceSqr;
            }
        }

        return resultTile;
    }


    // 타일의 속성(색 및 좌표)와 관련된 코드들입니다.

    private void RegisterTileCubeCoord()
    {
        foreach (var tile in allTiles)
        {
            tileMap[tile.CubeCoord] = tile;
        }
    }

    public void ChangeTileColorIfMoveable(bool isClicked)
    {
        if (isClicked)
        {
            playerMoveRange = GetMoveableTiles();
        }

        foreach (HexTile tile in playerMoveRange)
        {
            if (isClicked)
            {
                tile.PaintColor(tile.playerMoveRangeColor);
            }
            else
            {
                tile.ResetColor();
            }
        }
    }

    public void ResetTileColor()
    {
        foreach (HexTile tile in allTiles)
        {
            tile.ResetColor();
        }
    }

    // 보스의 타일과 관련된 코드들입니다. 

    public bool IsBossTile(HexTile tile)
    {
        if (boss.GetComponent<BossInteraction>().currentTile == tile || boss.GetComponent<BossInteraction>().neighborTile.Contains(tile))
        {
            return true;
        }
        return false;
    }

    public bool IsBossTile(List<HexTile> tiles)
    {
        foreach (HexTile tile in tiles)
            if (IsBossTile(tile))
                return true;
        return false;
    }

    public Color GetBossTileColor()
    {
        return boss.GetComponent<BossInteraction>().BossTileColor;
    }

    // BFS 를 사용하는 코드들입니다. 

    public List<HexTile> GetTilesWithinRange(HexTile startTile, int range)
    {
        List<HexTile> tilesInRange = new List<HexTile>();
        BFS(startTile, range, (tile, distance) =>
        {
            tilesInRange.Add(tile);
            return false; // Continue BFS
        });
        return tilesInRange;
    }

    public int GetTileDistance(Vector3Int start, Vector3Int end)
    {
        HexTile startTile = IsThereHexTileByCube(start);
        HexTile endTile = IsThereHexTileByCube(end);
        if (startTile != null && endTile != null)
        {
            return GetTileDistance(startTile, endTile);
        }

        return 0;
    }

    public int GetTileDistance(HexTile startTile, HexTile targetTile)
    {
        int distance = int.MaxValue;
        BFS(startTile, int.MaxValue, (tile, dist) =>
        {
            if (tile == targetTile)
            {
                distance = dist;
                return true; // Stop BFS
            }
            return false; // Continue BFS
        });
        return distance;
    }

    private void BFS(HexTile startTile, int maxDistance, System.Func<HexTile, int, bool> processTile)
    {
        Queue<HexTile> queue = new Queue<HexTile>();
        HashSet<HexTile> visited = new HashSet<HexTile>();
        queue.Enqueue(startTile);
        visited.Add(startTile);
        int distance = 0;

        while (queue.Count > 0)
        {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                HexTile tile = queue.Dequeue();
                if (processTile(tile, distance))
                {
                    return; // Stop BFS if processTile returns true
                }

                if (tile != null)
                {
                    foreach (HexTile neighbor in tile.neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }
            }
            distance++;
            if (distance > maxDistance)
            {
                break;
            }
        }
    }
}