using System.Collections.Generic;
using UnityEngine;

public class TileBackHelper : MonoBehaviour
{
    public HexTile GetBackCube(HexTile playerTile, HexTile targetTile)
    {
        return GetBackCube(playerTile, targetTile, 1);
    }

    public HexTile GetBackCube(HexTile playerTile, HexTile targetTile, int distance)
    {
        if (playerTile == null || targetTile == null) return null;

        // TargetTile -> PlayerTile 방향 벡터
        Vector3 dirToPlayer = (playerTile.transform.position - targetTile.transform.position).normalized;

        HexTile bestTile = null;
        float bestDot = -Mathf.Infinity;

        List<HexTile> tiles = HexTileManager.Instance.GetTilesWithinRange(playerTile, distance);

        foreach (HexTile tile in HexTileManager.Instance.GetTilesWithinRange(playerTile, distance - 1))
        {
            tiles.Remove(tile);
        }

        foreach (HexTile neighbor in tiles)
        {
            if (neighbor == null) continue;

            Vector3 dirToNeighbor = (neighbor.transform.position - playerTile.transform.position).normalized;
            float dot = Vector3.Dot(dirToPlayer, dirToNeighbor);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestTile = neighbor;
            }
        }

        // Dot 값이 너무 낮거나(bestTile이 null 포함) 타일이 없을 때 → playerTile 반환
        // 예: dot < 0.5는 방향이 너무 어긋난 경우라고 판단
        if (bestTile == null || bestDot < 0.5f)
        {
            return playerTile;
        }

        return bestTile;
    }

}