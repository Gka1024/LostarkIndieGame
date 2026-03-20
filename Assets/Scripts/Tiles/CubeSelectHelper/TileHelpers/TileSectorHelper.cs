using System.Collections.Generic;
using UnityEngine;

public class TileSectorHelper : MonoBehaviour
{
    public List<HexTile> GetSectorTiles(
        HexTile currentTile,
        HexTile facingTile,
        int radius,
        int angle)
    {
        List<HexTile> result = new();

        if (currentTile == null || facingTile == null)
            return result;

        // 정면 방향
        Vector3 forwardDir =
            (facingTile.transform.position - currentTile.transform.position).normalized;

        List<HexTile> tiles =
            HexTileManager.Instance.GetTilesWithinRange(currentTile, radius);

        float halfAngle = angle * 0.5f;

        foreach (HexTile tile in tiles)
        {
            if (tile == null || tile == currentTile)
                continue;

            Vector3 dir =
                (tile.transform.position - currentTile.transform.position).normalized;

            float tileAngle = Vector3.Angle(forwardDir, dir);

            if (tileAngle <= halfAngle)
            {
                result.Add(tile);
            }
        }

        return result;
    }
}