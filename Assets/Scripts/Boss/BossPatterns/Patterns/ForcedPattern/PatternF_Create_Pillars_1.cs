using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternF_Create_Pillars_1 : BossPattern
{ // 임포스터 패턴
    public PatternF_Create_Pillars_1()
    {
        turnGenerators.Add(MakeBossAir); // 0공중에 올라감
        turnGenerators.Add(MakePattern1); // 1낙하함
        turnGenerators.Add(MakePattern2); // 2기둥을 만듬
        turnGenerators.Add(MakeIdleTurn); //3 기둥을 만듬
        turnGenerators.Add(MakePattern3); // 4기둥을 부숨
        turnGenerators.Add(MakeIdleTurn); // 5
        turnGenerators.Add(MakePattern4); // 6기둥이 터짐
        turnGenerators.Add(MakeIdleTurn); // 7
        turnGenerators.Add(MakePattern5); // 8다른 기둥들이 터짐
    }

    private List<HexTile> brokenPillarTiles = new();

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 1)
        {
            ai.SetAirborne(false, HexTileManager.Instance.GetTileByCube(Vector3Int.zero));
        }

        if (currentTurn == 4 || currentTurn == 8)
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
        HexTile centertile = HexTileManager.Instance.GetTileByCube(new Vector3Int(0, 0, 0));


        List<HexTile> AttackRange = HexTileManager.Instance.GetTilesWithinRange(centertile, 2);

        return BossPatternBuilder.Create(AttackRange).SetDamage(10f).SetKnockback(3).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        ai.bossPatternHelper.CreatePillars(0);

        return MakeIdleTurn(ai);
    }

    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(GetBossTile(), GetPlayerTile(), 8, 60);

        foreach (HexTile tile in ai.bossPatternHelper.GetPillarSafeTilesLarge())
        {
            attackRange.Remove(tile);
        }

        return BossPatternBuilder.Create(attackRange).SetDamage(50f).Build();
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        List<HexTile> explosionTiles = new();

        foreach (HexTile tile in brokenPillarTiles)
        {
            explosionTiles.Add(tile);
            explosionTiles.AddRange(HexTileManager.Instance.GetTilesWithinRange(tile, 2));
        }

        return BossPatternBuilder
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
                explosionTiles.AddRange(HexTileManager.Instance.GetTilesWithinRange(tile, 3));
            }
        }

        return BossPatternBuilder
            .Create(explosionTiles)
            .SetDamage(60f)
            .Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}