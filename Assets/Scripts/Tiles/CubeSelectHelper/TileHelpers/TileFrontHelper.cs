using UnityEngine;

public class TileFrontHelper : MonoBehaviour
{
    public HexTile GetFrontCube(HexTile playerTile, HexTile targetTile)
    {
        if (playerTile == null || targetTile == null) return null;
        if (playerTile.neighbors == null) return playerTile;

        // PlayerTile -> TargetTile 방향 벡터
        Vector3 dirToTarget = (targetTile.transform.position - playerTile.transform.position).normalized;

        HexTile bestTile = null;
        float bestDot = -Mathf.Infinity;

        foreach (HexTile neighbor in playerTile.neighbors)
        {
            if (neighbor == null) continue;

            Vector3 dirToNeighbor = (neighbor.transform.position - playerTile.transform.position).normalized;
            float dot = Vector3.Dot(dirToTarget, dirToNeighbor);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestTile = neighbor;
            }
        }

        // 방향이 너무 어긋나거나 앞에 타일이 없을 경우 자기 자리 반환
        if (bestTile == null || bestDot < 0.5f)
        {
            return playerTile;
        }

        return bestTile;
    }
}