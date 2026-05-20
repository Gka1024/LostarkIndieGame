using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Cross_Whirlwind : BossPattern
{ // 십자로 찍고 휠윈드 (수정필)
    public PatternR_Cross_Whirlwind()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern2);
        
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();
        HexTile playerTile = ai.bossController.GetPlayerTile();
        List<HexTile> attackRange = TileRayHelper.GetCrossTiles(playerTile, curTile, 2);

        return BossPatternBuilder.Create(attackRange).SetDamage(30).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(curTile, 4);

        return BossPatternBuilder.Create(attackRange).SetDamage(40).SetKnockback(1).Build();   
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}