using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Cross_Whirlwind : BossPattern
{ // 십자로 찍고 휠윈드 (수정필)
    public PatternR_Cross_Whirlwind()
    {
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
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