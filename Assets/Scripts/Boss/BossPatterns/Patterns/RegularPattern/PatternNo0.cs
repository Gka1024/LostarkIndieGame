using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo0 : BossPattern
{ // 더미 패턴입니다. 
    public PatternNo0()
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
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = new();
        attackRange.Add(ai.bossController.GetCurrentTile());

        return BossPatternTurnBuilder.Create(attackRange).SetDamage(0).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}