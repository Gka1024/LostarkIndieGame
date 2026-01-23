using System.Collections.Generic;

public class PatternNo9 : BossPattern
{ // 휘적휘적 찍고 돌리기 패턴입니다. 
    public PatternNo9()
    {
        totalTurns = 5;

        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
        turnGenerators.Add(MakePattern4);
        turnGenerators.Add(MakePattern5);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai) => CreatePattern(ai, new[]
    {
        (2, 3, true), (3, 5, true),
        (4, 2, false), (4, 6, true),
        (5, 8, true), (5, 3, false)
    },
    damage: 40);

    private BossPatternTurnInfo MakePattern2(BossAI ai) => CreatePattern(ai, new[]
    {
        (2, 3, false), (3, 5, false),
        (4, 2, true), (4, 6, false),
        (5, 8, false), (5, 3, true)
    },
    damage: 40);

    private BossPatternTurnInfo MakePattern3(BossAI ai) => CreatePattern(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, true), (3, 3, false),
        (4, 3, true), (4, 3, false),
        (5, 3, true), (5, 3, false),
        (6, 2, true), (6, 2, false)
    },
    damage: 50, isDown: true);  // 예시: 다운 효과 추가

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern5(BossAI ai) => CreatePattern(ai, new[]
    {
        (4, 24, true),
        (5, 30, true),
        (6, 36, true)
    },
    damage: 60, isKnockback: true, knockbackDistance: 2);

    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 애니메이션, 사운드 재생 등
    }
}
