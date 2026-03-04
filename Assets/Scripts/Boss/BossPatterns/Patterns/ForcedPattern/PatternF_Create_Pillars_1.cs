using System.Collections.Generic;
using UnityEngine;

public class PatternF_Create_Pillars_1 : BossPattern
{ // 임포스터 패턴
    public PatternF_Create_Pillars_1()
    {
        turnGenerators.Add(MakeIdleTurn); // 공중에 올라감
        turnGenerators.Add(MakePattern1); // 낙하함
        turnGenerators.Add(MakePattern2); // 기둥을 만듬
        turnGenerators.Add(MakePattern3); // 기둥을 부숨
        turnGenerators.Add(MakePattern4); // 기둥이 터짐
        turnGenerators.Add(MakePattern5); // 다른 기둥들이 터짐
    }

    private List<HexTile> brokenPillarTiles = new();

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn != 3 || currentTurnInfo == null)
            return;

        List<HexTile> pillarsToBreak = new();

        foreach (HexTile tile in currentTurnInfo.TargetTiles)
        {
            if (tile.currentTileState == TileState.IsPillar)
                pillarsToBreak.Add(tile);
        }

        foreach (HexTile pillar in pillarsToBreak)
        {
            brokenPillarTiles.Add(pillar);
            GameManager.Instance.objectManager
                .DestroyObjectByTile(pillar);
        }
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> AttackRange = new();
        HexTile centertile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        AttackRange.AddRange(centertile.neighbors);
        AttackRange.Add(centertile);

        return BossPatternTurnBuilder.Create(AttackRange).SetDamage(10f).SetKnockback(3).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        ai.bossPatternHelper.CreatePillars();

        return MakeIdleTurn(ai);
    }

    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        var patterns = new (int direction, int count, bool clockwise)[]
        {
            (2, 2, true), (2, 2, false),
            (3, 3, true), (3, 3, false),
            (4, 3, true), (4, 3, false),
            (5, 3, true), (5, 3, false),
        };

        List<HexTile> attackRange = PatternUtility.GetAttackRangeByDistance(ai, patterns);

        return BossPatternTurnBuilder.Create(attackRange).SetDamage(50f).Build();
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        List<HexTile> explosionTiles = new();

        foreach (HexTile tile in brokenPillarTiles)
        {
            explosionTiles.Add(tile);
            explosionTiles.AddRange(tile.neighbors);
        }

        return BossPatternTurnBuilder
            .Create(explosionTiles)
            .SetDamage(40f)
            .Build();
    }

    private BossPatternTurnInfo MakePattern5(BossAI ai)
    {
        List<HexTile> explosionTiles = new();

        ObjectManager objectManager = GameManager.Instance.objectManager;

        // 모든 Pillar 타일 그룹 순회
        foreach (TileSpecific type in System.Enum.GetValues(typeof(TileSpecific)))
        {
            if (!type.ToString().Contains("Pillar"))
                continue;

            List<HexTile> pillarTiles = objectManager.GetTiles(type);
            if (pillarTiles == null) continue;

            foreach (HexTile tile in pillarTiles)
            {
                if (tile.currentTileState != TileState.IsPillar)
                    continue;

                explosionTiles.Add(tile);
                explosionTiles.AddRange(tile.neighbors);
            }
        }

        return BossPatternTurnBuilder
            .Create(explosionTiles)
            .SetDamage(60f)
            .Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}