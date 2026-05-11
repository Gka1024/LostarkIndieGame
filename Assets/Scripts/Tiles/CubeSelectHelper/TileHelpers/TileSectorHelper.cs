using System.Collections.Generic;
using UnityEngine;

public class TileSectorHelper : MonoBehaviour
{
    /// <summary>
    /// 섹터 타일들을 가져옵니다.
    /// </summary>
    /// <param name="currentTile">기준 중심 타일</param>
    /// <param name="baseFacingTile">기본 정면 기준 타일</param>
    /// <param name="radius">반지름</param>
    /// <param name="sectorAngle">부채꼴의 넓이 (각도)</param>
    /// <param name="rotationOffset">기본 정면 기준 추가 회전 각도 (0이면 정면)</param>
    public List<HexTile> GetSectorTiles(
        HexTile currentTile,
        HexTile baseFacingTile,
        int radius,
        int sectorAngle,
        float rotationOffset = 0f)
    {
        List<HexTile> result = new();

        if (currentTile == null || baseFacingTile == null)
            return result;

        // 1. 기본 정면 방향 계산
        Vector3 baseDir = (baseFacingTile.transform.position - currentTile.transform.position).normalized;

        // 2. [발전] 기본 방향을 rotationOffset만큼 회전시킴 (Y축 기준 회전)
        // 보스 패턴에서 "오른쪽/왼쪽 90도 공격" 등을 구현할 때 핵심입니다.
        Vector3 finalForwardDir = Quaternion.Euler(0, rotationOffset, 0) * baseDir;

        // 3. 범위 내 모든 타일 가져오기
        List<HexTile> tiles = HexTileManager.Instance.GetTilesWithinRange(currentTile, radius);

        float halfAngle = sectorAngle * 0.5f;

        foreach (HexTile tile in tiles)
        {
            if (tile == null || tile == currentTile)
                continue;

            // 타일로 향하는 방향
            Vector3 targetDir = (tile.transform.position - currentTile.transform.position).normalized;

            // 4. 최종 회전된 방향과 타일 방향 사이의 각도 계산
            float angleDiff = Vector3.Angle(finalForwardDir, targetDir);

            if (angleDiff <= halfAngle)
            {
                result.Add(tile);
            }
        }

        return result;
    }
}