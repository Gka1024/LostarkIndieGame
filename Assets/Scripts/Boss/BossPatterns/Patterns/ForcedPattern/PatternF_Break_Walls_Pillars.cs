using System.Collections.Generic;
using UnityEngine;

public class PatternF_Break_All_Walls_Pillars : BossPattern
{  // 벽 파괴 패턴
    public PatternF_Break_All_Walls_Pillars()
    {
        turnGenerators.Add(MakePattern1); // 2
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        ai.bossPatternHelper.BreakAllWalls();
        ai.bossPatternHelper.BreakAllPillars();
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
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