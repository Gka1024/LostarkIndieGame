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

    private BossPatternTurnInfo MakePattern1(BossAI ai) => PatternUtility.CreatePatternByDistance(ai, new[]
    {
        (2, 12, true),
        (3, 18, true),
        (4, 24, true)
    },
   damage: 30, knockbackDistance: 2);

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}