using System.Collections.Generic;
using UnityEngine;

public class PatternF_Chain_Destruction_Fist : BossPattern
{ // 연환파신권 패턴
    public PatternF_Chain_Destruction_Fist()
    {
        turnGenerators.Add(MakeIdleTurn); // 2
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
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