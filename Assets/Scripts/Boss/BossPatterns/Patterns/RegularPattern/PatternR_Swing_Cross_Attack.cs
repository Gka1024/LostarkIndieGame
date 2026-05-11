using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Swing_Cross_Attack : BossPattern
{ // 십자로 찍기 패턴입니다.
    public PatternR_Swing_Cross_Attack()
    {
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), playerTile, 120, 2);

        return BossPatternBuilder.Create(attackRange).SetDamage(30).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();
        HexTile playerTile = ai.bossController.GetPlayerTile();
        List<HexTile> attackRange = TileRayHelper.GetCrossTiles(playerTile, curTile, 2);

        return BossPatternBuilder.Create(attackRange).SetDamage(1).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}