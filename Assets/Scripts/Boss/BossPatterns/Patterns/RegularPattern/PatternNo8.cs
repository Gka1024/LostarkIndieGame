using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo8 : BossPattern
{ // 부채꼴 두번 찍기 패턴입니다.
    public PatternNo8()
    {
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
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
    },
    damage: 50, downDuration: 2);

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}