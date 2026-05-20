using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Portal_Rush : BossPattern
{ // 포탈 돌진

    public PatternR_Portal_Rush()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeBossAir);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add((ai) => MakeBossDown(ai));
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        // 필요 시 패턴 시작 시점에 초기화할 변수 추가
    }

    // 1턴: 돌진 경로 공격
    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var facing = HexTileManager.Instance.GetTileByCube(current.CubeCoord + new Vector3Int(1, 0, -1));

        if (facing == null)
            return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();

        // 이동 방향 벡터 계산
        Vector3Int moveVector = facing.CubeCoord - current.CubeCoord;

        List<HexTile> moveLines = new();
        moveLines.Add(current);

        HexTile nextTile = HexTileManager.Instance.GetTileByCube(current.CubeCoord + moveVector);

        while (nextTile != null)
        {
            moveLines.Add(nextTile);
            current = nextTile;
            nextTile = HexTileManager.Instance.GetTileByCube(current.CubeCoord + moveVector);
        }

        // 공격 범위 계산
        HashSet<HexTile> attackRangeSet = new();

        foreach (HexTile tile in moveLines)
        {
            attackRangeSet.UnionWith(HexTileManager.Instance.GetTilesWithinRange(tile, 1));
        }

        return BossPatternBuilder.Create(attackRangeSet.ToList()).SetDamage(40).Build();
    }

    // 2턴: 포탈에서 플레이어 방향으로 광선 공격
    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        // 랜덤 포탈 타일 선택
        HexTile randomTile = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());

        // 랜덤 포탈에서 플레이어 방향으로 2칸 광선
        List<HexTile> result = TileRayHelper.GetRayTiles(randomTile, playerTile, 2, true);

        return BossPatternBuilder.Create(result).SetDamage(20).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}
