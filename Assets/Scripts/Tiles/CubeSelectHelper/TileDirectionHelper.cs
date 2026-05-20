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
    public TileSectorHelper sectorHelper;

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

    public HexTile GetFrontTile(HexTile playerTile, HexTile targetTile, int distance = 0)
    {
        return frontHelper.GetFrontTile(playerTile, targetTile, distance);
    }

    public HexTile GetBackTile(HexTile playerTile, HexTile targetTile, int distance)
    {
        return backHelper.GetBackTile(playerTile, targetTile, distance);
    }

    public List<HexTile> GetDistanceTiles(HexTile currentTile, HexTile facingTile, int distance, int tileCount, bool isClockWise = true)
    {
        return distanceHelper.GetClockWiseTiles(currentTile, facingTile, distance, tileCount, isClockWise);
    }

    public List<HexTile> GetSectorTiles(HexTile currentTile, HexTile facingTile, int radius, int angle, float rotationOffset = 0f)
    {
        return sectorHelper.GetSectorTiles(currentTile, facingTile, radius, angle, rotationOffset);
    } 

}