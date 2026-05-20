using System.Collections.Generic;

public class PatternR_Swing_And_Spin : BossPattern
{ // 휘적휘적 찍고 돌리기 패턴입니다. 
    public PatternR_Swing_And_Spin()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern5);
        turnGenerators.Add(MakePattern6);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
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
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();
        HexTile attackTile = TileDirectionHelper.Instance.GetFrontTile(bossTile, playerTile, 4);

        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(attackTile, 2);
        return BossPatternBuilder.Create(attackRange).SetDamage(40).SetKnockback(1).Build();
    }

    private BossPatternTurnInfo MakePattern5(BossAI ai)
    {
        HexTile bossTile = ai.bossController.GetCurrentTile();
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(bossTile, 4);

        return BossPatternBuilder.Create(attackRange).SetDamage(50).SetKnockback(1).Build();
    }

    private BossPatternTurnInfo MakePattern6(BossAI ai)
    {
        HexTile bossTile = ai.bossController.GetCurrentTile();
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(bossTile, 6);

        return BossPatternBuilder.Create(attackRange).SetDamage(50).SetKnockback(1).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 애니메이션, 사운드 재생 등
    }
}
