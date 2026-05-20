using System.Collections.Generic;
using UnityEngine;

public class PatternF_Create_Pillars_3 : BossPattern
{ // 임포스터 패턴3 - 2지파 후 사용하는 임포스터 패턴 / 이후 연환파신권으로 연계
    public PatternF_Create_Pillars_3()
    {
        turnGenerators.Add(MakePattern1); // 0 : 기둥 생성
        turnGenerators.Add(MakePattern2); // 1 : 광범위 공격
        turnGenerators.Add(MakePattern3); // 2 : 쪼기
        turnGenerators.Add(MakePattern3); // 3 : 쪼기 
        turnGenerators.Add(MakePattern4); // 4 : 공격
        turnGenerators.Add(MakePattern5); // 5 : 터지고
        turnGenerators.Add(MakeIdleTurn); // 6 : 터지고
        turnGenerators.Add(MakePattern5); // 7 : 다른거 부수기
    }
    private List<HexTile> brokenPillarTiles = new();

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 4 && currentTurnInfo != null)
        {
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
                    .DestroyObjectBySpecificTile(pillar);
            }
        }

    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        ai.bossPatternHelper.CreatePillars(1);

        return MakeIdleTurn(ai);
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        List<HexTile> attackRange = HexTileManager.Instance.GetAllTiles();
        List<HexTile> pillarTiles = ai.bossPatternHelper.GetPillarTiles(1);
        List<HexTile> pillarSafe = ai.bossPatternHelper.GetPillarSafeTilesSmall();

        foreach (HexTile tile in pillarTiles)
        {
            attackRange.Remove(tile);
        }

        foreach (HexTile tile in pillarSafe)
        {
            attackRange.Remove(tile);
        }

        return BossPatternBuilder.Create(attackRange).SetDamage(50f).SetKnockback(2).Build();
    }

    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(
            ai.bossInteraction.GetCurrentTile(), Player.Instance.move.GetCurrentTile(), 5, 100);

        return BossPatternBuilder.Create(attackRange).SetDamage(0f).Build();
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(
            ai.bossInteraction.GetCurrentTile(), Player.Instance.move.GetCurrentTile(), 5, 100);

        return BossPatternBuilder.Create(attackRange).SetDamage(40f).SetKnockback(2).Build();
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
                explosionTiles.AddRange(HexTileManager.Instance.GetTilesWithinRange(tile, 2));
            }
        }

        return BossPatternBuilder
            .Create(explosionTiles)
            .SetDamage(60f)
            .Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        throw new System.NotImplementedException();
    }
}