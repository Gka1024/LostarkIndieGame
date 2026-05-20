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

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, playerTile, 4, 120, 30);
        return BossPatternBuilder.Create(attackRange).SetDamage(20).SetKnockback(1).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, playerTile, 4, 120, 30);
        return BossPatternBuilder.Create(attackRange).SetDamage(20).SetKnockback(1).Build();
    }
    
    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        List<HexTile> attackRange = new();

        HexTile bossTile = ai.bossController.GetCurrentTile();
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile tileAttackCenter = TileDirectionHelper.Instance.frontHelper.GetFrontTile(bossTile, playerTile);

        attackRange.AddRange(TileRayHelper.GetHexagramTiles(tileAttackCenter));

        return BossPatternBuilder.Create(attackRange).SetDamage(70).Build();
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        var attackTiles = HexTileManager.Instance.GetAllTiles();

        return BossPatternBuilder.Create(attackTiles).SetDamage(0).SetSpecial().Build();
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