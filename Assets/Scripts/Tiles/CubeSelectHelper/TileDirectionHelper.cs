using System.Collections.Generic;
using UnityEngine;

public class TileDirectionHelper : MonoBehaviour
{
    public static TileDirectionHelper Instance { get; private set; }

    public HexTileManager tileManager;

    public TileFrontHelper frontHelper;
    public TileBackHelper backHelper;
    public TileDistanceHelper distanceHelper;
    public TileRayHelper tileRayHelper;

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
    }

    public HexTile GetFrontTile(HexTile playerTile, HexTile targetTile)
    {
        return frontHelper.GetFrontCube(playerTile, targetTile);
    }

    public HexTile GetBackTile(HexTile playerTile, HexTile targetTile, int distance)
    {
        return backHelper.GetBackCube(playerTile, targetTile, distance);
    }

    public List<HexTile> GetDistanceTiles(HexTile currentTile, HexTile facingTile, int distance, int tileCount, bool isClockWise = true)
    {
        return distanceHelper.GetClockWiseTiles(currentTile, facingTile, distance, tileCount, isClockWise);
    }

}