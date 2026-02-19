using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo11 : BossPattern
{ // 4번찍기 패턴입니다. 
    public PatternNo11()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern2);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai) => PatternUtility.CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
    },
     damage: 50);

    private BossPatternTurnInfo MakePattern2(BossAI ai) => PatternUtility.CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
    },
    damage: 60, downDuration: 3);

    private BossPatternTurnInfo MakePattern3(BossAI ai) => PatternUtility.CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
        (5, 4, false), (5, 4, true),
    },
    damage: 60);


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}