using System;
using System.Collections.Generic;
using UnityEngine;

public class TileDistanceHelper : MonoBehaviour
{ // 기준 타일에서 일정 거리만큼 떨어져 있는 타일들을 시계방향으로 반환하는 코드입니다.
    public List<HexTile> allTiles;

    void Start()
    {
        allTiles = HexTileManager.Instance.GetAllTiles();
    }

    public static readonly Dictionary<CubeDirection, Vector3Int> CubeDirectionToVectors = new()
    {
        { CubeDirection.Left,     new Vector3Int(-1, 1, 0) },
        { CubeDirection.LeftUp,   new Vector3Int(-1, 0, 1) },
        { CubeDirection.RightUp,  new Vector3Int(0, -1, 1) },
        { CubeDirection.Right,    new Vector3Int(1, -1, 0) },
        { CubeDirection.RightDown,new Vector3Int(1, 0, -1) },
        { CubeDirection.LeftDown, new Vector3Int(0, 1, -1) },
    };

    public static readonly Dictionary<Vector3Int, CubeDirection> VectorToCubeDirections = new()
    {
    { new Vector3Int(-1, 1, 0),  CubeDirection.Left },
    { new Vector3Int(-1, 0, 1),  CubeDirection.LeftUp },
    { new Vector3Int(0, -1, 1),  CubeDirection.RightUp },
    { new Vector3Int(1, -1, 0),  CubeDirection.Right },
    { new Vector3Int(1, 0, -1),  CubeDirection.RightDown },
    { new Vector3Int(0, 1, -1),  CubeDirection.LeftDown },
    };


    public enum CubeDirection
    {
        Left,
        LeftUp,
        RightUp,
        Right,
        RightDown,
        LeftDown
    }

    public static readonly Vector3Int[] CubeDirectionClockwise = new Vector3Int[]
    {
        new (-1, 1, 0), // 좌
        new (-1, 0, 1), // 좌상
        new (0, -1, 1), // 우상
        new (1, -1, 0), // 우
        new (1, 0, -1), // 우하
        new (0, 1, -1), // 좌하
    };

    /// <summary>
    /// 시계방향 타일 반환
    /// </summary>
    public List<HexTile> GetClockWiseTiles(HexTile currentTile, HexTile facingTile, int distance, int tileCount, bool isClockWise = true)
    {
        return GetArcTiles(currentTile, facingTile, distance, tileCount, isClockWise);
    }

    /// <summary>
    /// 내부 공통 처리 (시계/반시계 방향) - facingTile은 currentTile의 2칸 이상 떨어져있어야 합니다.
    /// </summary>
    private List<HexTile> GetArcTiles(HexTile currentTile, HexTile facingTile, int distance, int tileCount, bool isClockwise)
    {
        List<HexTile> result = new List<HexTile>();
        List<HexTile> ring = GetRingPositions(currentTile.CubeCoord, distance);

        List<HexTile> startTiles = FindAllClosestTilesOnLine(currentTile, facingTile, ring);
        HexTile startTile = ChooseBestStartTile(startTiles, currentTile, isClockwise);

        if (startTile == null)
        {
            Debug.LogWarning("시작 타일을 찾지 못했습니다.");
            return result;
        }

        result.Add(startTile);

        // 초기 방향 벡터 (current -> facing)
        Vector3Int relativeCoord = facingTile.CubeCoord - currentTile.CubeCoord;
        Vector3Int facingCoord = NormalizeCubeDirection(relativeCoord);

        if (!VectorToCubeDirections.TryGetValue(facingCoord, out CubeDirection currentDir))
        {
            currentDir = GetNearestCubeDirection(facingCoord, isClockwise);
        }

        CubeDirection nextDir = GetNextDirection(currentDir, isClockwise, isStart: true);
        Vector3Int nextVector = CubeDirectionToVectors[nextDir];

        HexTile pointTile = HexTileManager.Instance.IsThereHexTileByCube(startTile.CubeCoord + nextVector);

        while (result.Count < tileCount && pointTile != null && ring.Contains(pointTile))
        {
            result.Add(pointTile);

            // 꼭짓점인지 변 중간인지 확인해서 다음 방향 계산
            bool isPoint = IsPointEnd(pointTile.CubeCoord);
            CubeDirection stepDir = GetNextDirection(nextDir, isClockwise, isPoint);
            nextVector = CubeDirectionToVectors[stepDir];

            nextDir = stepDir;
            pointTile = HexTileManager.Instance.IsThereHexTileByCube(pointTile.CubeCoord + nextVector);
        }

        return result;
    }


    /// <summary>
    /// 특정 거리 링 타일 좌표 (시계방향)
    /// </summary>
    public List<HexTile> GetRingPositions(Vector3Int center, int radius)
    {
        List<HexTile> tiles = new();

        foreach (HexTile tile in allTiles)
        {
            // center 기준 상대 좌표로 거리 계산
            Vector3Int relative = tile.CubeCoord - center;
            if (GetTileDistance(relative) == radius) tiles.Add(tile);
        }

        return tiles;
    }

    private static CubeDirection GetNearestCubeDirection(Vector3Int dir, bool isClockwise)
    {
        if (dir == Vector3.zero) throw new ArgumentException("방향 벡터가 0입니다.");

        if (dir == new Vector3Int(0, 0, 1)) // 12시 방향인 경우
        {
            if (isClockwise) return CubeDirection.LeftUp;
            else return CubeDirection.RightUp;
        }
        else if (dir == new Vector3Int(0, -1, 0)) // 1~2시 방향인 경우
        {
            if (isClockwise) return CubeDirection.RightUp;
            else return CubeDirection.Right;
        }
        else if (dir == new Vector3Int(1, 0, 0)) // 4~5시 방향인 경우
        {
            if (isClockwise) return CubeDirection.Right;
            else return CubeDirection.RightDown;
        }
        else if (dir == new Vector3Int(0, 0, -1)) // 6시 방향인 경우
        {
            if (isClockwise) return CubeDirection.RightDown;
            else return CubeDirection.LeftDown;
        }
        else if (dir == new Vector3Int(0, 1, 0)) // 7~8시 방향인 경우
        {
            if (isClockwise) return CubeDirection.LeftDown;
            else return CubeDirection.Left;
        }
        else if (dir == new Vector3Int(-1, 0, 0)) // 10~11시 방향인 경우
        {
            if (isClockwise) return CubeDirection.Left;
            else return CubeDirection.LeftUp;
        }

        throw new ArgumentException("방향이 없습니다.");
    }

    private static CubeDirection ChooseBestStartTileDirection(Vector3Int dir, bool isClockwise)
    {
        if (dir == Vector3.zero) throw new ArgumentException("방향 벡터가 0입니다.");

        if (dir == new Vector3Int(0, 0, 1)) // 12시 방향인 경우
        {
            if (isClockwise) return CubeDirection.Left;
            else return CubeDirection.Right;
        }
        else if (dir == new Vector3Int(0, -1, 0)) // 1~2시 방향인 경우
        {
            if (isClockwise) return CubeDirection.LeftUp;
            else return CubeDirection.RightDown;
        }
        else if (dir == new Vector3Int(1, 0, 0)) // 4~5시 방향인 경우
        {
            if (isClockwise) return CubeDirection.RightUp;
            else return CubeDirection.LeftDown;
        }
        else if (dir == new Vector3Int(0, 0, -1)) // 6시 방향인 경우
        {
            if (isClockwise) return CubeDirection.Right;
            else return CubeDirection.Left;
        }
        else if (dir == new Vector3Int(0, 1, 0)) // 7~8시 방향인 경우
        {
            if (isClockwise) return CubeDirection.RightDown;
            else return CubeDirection.LeftUp;
        }
        else if (dir == new Vector3Int(-1, 0, 0)) // 10~11시 방향인 경우
        {
            if (isClockwise) return CubeDirection.LeftDown;
            else return CubeDirection.RightUp;
        }

        throw new ArgumentException("방향이 없습니다.");
    }

    private HexTile ChooseBestStartTile(List<HexTile> startTiles, HexTile currentTile, bool isClockwise)
    {
        if (startTiles == null || startTiles.Count == 0)
        {
            return null;
        }

        if (startTiles.Count == 1)
        {
            return startTiles[0];
        }

        HexTile checkTile = !IsPointEnd(startTiles[0].CubeCoord) ? startTiles[0] : startTiles[1];
        Vector3Int dir = NormalizeCubeDirection(checkTile.CubeCoord - currentTile.CubeCoord);
        CubeDirection approxDir = ChooseBestStartTileDirection(dir, isClockwise);
        Vector3Int nextVec = CubeDirectionToVectors[approxDir];
        Vector3Int targetCoord = checkTile.CubeCoord + nextVec;
        HexTile targetTile = HexTileManager.Instance.IsThereHexTileByCube(targetCoord);

        if (startTiles.Contains(targetTile))
        {
            return targetTile;
        }
        else
        {
            return checkTile;
        }
    }

    private List<HexTile> FindAllClosestTilesOnLine(HexTile fromTile, HexTile toTile, List<HexTile> ring)
    {
        Vector3 start = fromTile.transform.position;
        Vector3 end = toTile.transform.position;
        Vector3 forward = (end - start).normalized;

        List<HexTile> closestTiles = new();
        float closestDistance = float.MaxValue;

        foreach (var tile in ring)
        {
            Vector3 tilePos = tile.transform.position;

            // 방향 필터
            Vector3 dirToTile = (tilePos - start).normalized;
            float dot = Vector3.Dot(forward, dirToTile);
            if (dot < 0.7f) continue;

            // 수선의 발 기준 거리
            Vector3 projected = ProjectPointOnLine(start, end, tilePos);
            float dist = Vector3.Distance(tilePos, projected);

            if (Mathf.Approximately(dist, closestDistance))
            {
                closestTiles.Add(tile);
            }
            else if (dist < closestDistance)
            {
                closestTiles.Clear();
                closestTiles.Add(tile);
                closestDistance = dist;
            }
        }

        return closestTiles;
    }

    private Vector3 ProjectPointOnLine(Vector3 a, Vector3 b, Vector3 point)
    {
        Vector3 ab = (b - a).normalized;
        float length = Vector3.Dot(point - a, ab);
        return a + ab * length;
    }

    public static Vector3Int NormalizeCubeDirection(Vector3Int dir)
    {
        if (dir == Vector3Int.zero) return Vector3Int.zero;

        int maxAbs = Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z));
        return new Vector3Int(dir.x / maxAbs, dir.y / maxAbs, dir.z / maxAbs);
    }

    private int GetTileDistance(Vector3Int vec)
    {
        int result = Math.Max(Math.Abs(vec.x), Math.Abs(vec.y));
        result = Math.Max(result, Math.Abs(vec.z));

        return result;
    }

    private bool IsPointEnd(Vector3Int vec)
    {
        int x = vec.x;
        int y = vec.y;
        int z = vec.z;

        if (Math.Abs(x) == 0 || Math.Abs(y) == 0 || Math.Abs(z) == 0) return true;
        return false;
    }

    public static CubeDirection GetNextDirection(CubeDirection current, bool clockwise = true, bool isPoint = false, bool isStart = false)
    {
        if (!isPoint && !isStart)
        {
            return current;
        }

        CubeDirection[] directions = new CubeDirection[]
        {
        CubeDirection.Left,
        CubeDirection.LeftUp,
        CubeDirection.RightUp,
        CubeDirection.Right,
        CubeDirection.RightDown,
        CubeDirection.LeftDown
        };

        int index = System.Array.IndexOf(directions, current);
        if (index == -1)
        {
            Debug.LogWarning("해당 방향이 목록에 없음");
            return current;
        }

        int step = isStart ? 2 : 1;
        int nextIndex;

        if (clockwise)
            nextIndex = (index + step) % directions.Length;
        else
            nextIndex = (index - step + directions.Length) % directions.Length;

        return directions[nextIndex];
    }

}
