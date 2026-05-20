using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Whirlwind : BossPattern
{ // 휠윈드
    public PatternR_Whirlwind()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile bossTile = ai.bossController.GetCurrentTile();
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(bossTile, 4);

        return BossPatternBuilder.Create(attackRange).SetDamage(30).SetKnockback(1).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}