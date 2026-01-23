using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileRayHelper : MonoBehaviour
{
    public static List<HexTile> GetRayTiles(HexTile startTile, HexTile targetTile, int width, bool isOverable)
    {
        if (startTile == null || targetTile == null)
        {
            Debug.LogError($"Tile null! starttile : {startTile} | targettile : {targetTile}");

            return null;
        }


        Vector3 forwardDir = (targetTile.transform.position - startTile.transform.position).normalized;

        int tileDistance = HexTileManager.Instance.GetTileDistance(startTile, targetTile);
        List<HexTile> tilesInRange = HexTileManager.Instance.GetTilesWithinRange(startTile, tileDistance);

        HashSet<HexTile> resultTiles = new();

        foreach (HexTile tile in tilesInRange)
        {
            Vector3 tileDir = (tile.transform.position - startTile.transform.position).normalized;
            if (Vector3.Dot(forwardDir, tileDir) > 0.8f && IsTileWithinWidth(startTile, targetTile, tile, width))
            {
                resultTiles.Add(tile);
            }
        }

        if (isOverable)
        {
            // Add tiles that are not in the range but are within the width
            foreach (HexTile tile in HexTileManager.Instance.GetAllTiles())
            {
                if (!resultTiles.Contains(tile) && IsTileWithinWidth(startTile, targetTile, tile, width))
                {
                    resultTiles.Add(tile);
                }
            }
        }

        return resultTiles.ToList();
    }

    public static (List<HexTile> rayTiles, HexTile wallTile) GetRayTilesForRush(HexTile startTile, HexTile targetTile, int width, bool stopAtWall = false)
    {
        List<HexTile> rayTiles = GetTileListInDirection(startTile, targetTile, GetRayTiles(startTile, targetTile, width, true));
        HexTile stopTile = null;
        int distanceFromWall = int.MaxValue;

        foreach (HexTile tile in rayTiles)
        { // 레이 타일들을 돌면서 iswall이면 stoptile에 넣고 벽까지의 거리 설정
            if (tile.currentTileState == TileState.IsWall)
            {
                int distance = HexTileManager.Instance.GetTileDistance(startTile, tile);
                if (distance < distanceFromWall)
                {
                    distanceFromWall = distance;
                    stopTile = tile;
                }
            }
        }

        List<HexTile> resultTiles = new();

        if (stopTile != null && stopAtWall)
        { // 벽에 걸린 경우
            foreach (HexTile tile in rayTiles)
            {
                if (HexTileManager.Instance.GetTileDistance(startTile, tile) <= distanceFromWall)
                {
                    resultTiles.Add(tile);
                }
            }
        }
        else // 벽에 걸리지 않는 경우
        {
            resultTiles = rayTiles;
            int tileDistance = 0;

            foreach (HexTile tile in resultTiles)
            { // 가장 먼 타일 반환
                int curTileDis = HexTileManager.Instance.GetTileDistance(startTile, tile);
                if (curTileDis > tileDistance)
                {
                    tileDistance = curTileDis;
                    stopTile = tile;
                }
            }

        }

        return (resultTiles, stopTile);
    }

    public HexTile GetRayNextTile(HexTile startTile, HexTile targetTile, int index)
    {
        List<HexTile> rayTiles = GetRayTiles(startTile, targetTile, 1, false);

        foreach (HexTile tile in rayTiles)
        {
            if (HexTileManager.Instance.GetTileDistance(startTile, tile) == index)
            {
                return tile;
            }
        }

        return startTile;
    }

    private static bool IsTileWithinWidth(HexTile start, HexTile end, HexTile target, float width)
    {
        float distance = DistancePointToLine(target.transform.position, start.transform.position, end.transform.position);
        return distance <= width;
    }

    private static float DistancePointToLine(Vector3 point, Vector3 start, Vector3 end)
    {
        Vector3 lineDir = (end - start).normalized;
        float projLength = Vector3.Dot(point - start, lineDir);
        Vector3 closestPoint = start + projLength * lineDir;
        return Vector3.Distance(point, closestPoint);
    }

    private static List<HexTile> GetTileListInDirection(HexTile startTile, HexTile targetTile, List<HexTile> tileList)
    {
        List<HexTile> result = new();

        if (startTile == null || targetTile == null || tileList == null) return result;

        Vector3 tileDir = targetTile.transform.position - startTile.transform.position;

        foreach (HexTile tile in tileList)
        {
            if (IsVector3InSameDirection(tileDir, tile.transform.position - startTile.transform.position))
            {
                result.Add(tile);
            }
        }


        return result;
    }

    private static bool IsVector3InSameDirection(Vector3 vec3A, Vector3 vec3B)
    {
        Vector3 a = vec3A.normalized;
        Vector3 b = vec3B.normalized;

        if ((a + b).magnitude > 1.5f) // 대락 수직보다 조금 안쪽. 최대치는 2 (완전히 같은 방향)
        {
            return true;
        }

        return false;
    }
}
