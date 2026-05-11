using System.Collections.Generic;
using UnityEngine;

public class PatternF_Brandish_Annihilate : BossPattern
{ // 휘적휘적 후 전멸
    public PatternF_Brandish_Annihilate()
    {
        turnGenerators.Add(MakePattern1); // 0
        turnGenerators.Add(MakePattern2); // 1
        turnGenerators.Add(MakePattern0); // 2
        turnGenerators.Add(MakePattern3); // 3
        turnGenerators.Add(MakePattern0); // 4
        turnGenerators.Add(MakePattern4); // 5
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai) =>
        PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 3, true), (3, 5, true),
            (4, 2, false), (4, 6, true),
            (5, 8, true), (5, 3, false)
        },
        damage: 40);

    private BossPatternTurnInfo MakePattern2(BossAI ai) =>
        PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 3, false), (3, 5, false),
            (4, 2, true), (4, 6, false),
            (5, 8, false), (5, 3, true)
        },
        damage: 40);

    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        List<HexTile> attackRange = new();

        HexTile bossTile = ai.bossController.GetCurrentTile();
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile tileAttackCenter = TileDirectionHelper.Instance.frontHelper.GetFrontTile(bossTile, playerTile);

       attackRange.AddRange( TileRayHelper.GetHexagramTiles(tileAttackCenter));

        return BossPatternBuilder.Create(attackRange).SetDamage(1).Build();
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        var attackTiles = HexTileManager.Instance.GetAllTiles();

        return BossPatternBuilder.Create(attackTiles).SetDamage(1).SetSpecial().Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {
        // 필요 시 정리
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}