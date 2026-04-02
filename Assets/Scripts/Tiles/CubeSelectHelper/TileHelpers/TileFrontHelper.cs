using UnityEngine;

public class TileFrontHelper : MonoBehaviour
{
    public HexTile GetFrontTile(HexTile mainTile, HexTile targetTile)
    {
        if (mainTile == null || targetTile == null) return null;
        if (mainTile.neighbors == null) return mainTile;

        // PlayerTile -> TargetTile 방향 벡터
        Vector3 dirToTarget = (targetTile.transform.position - mainTile.transform.position).normalized;

        HexTile bestTile = null;
        float bestDot = -Mathf.Infinity;
 
        foreach (HexTile neighbor in mainTile.neighbors)
        {
            if (neighbor == null) continue;

            Vector3 dirToNeighbor = (neighbor.transform.position - mainTile.transform.position).normalized;
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
            return mainTile;
        }

        return bestTile;
    }
}