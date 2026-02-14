using UnityEngine;

public class BossPatternPhase1 : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterPatterns()
    {
        RegisterRegularPattern(new PatternNo9());
        RegisterRegularPattern(new AssignedPatternNo1());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new AssignedPatternNo1());
        RegisterOpeningPattern(new AssignedPatternNo2());
        RegisterOpeningPattern(new AssignedPatternNo3());
        RegisterOpeningPattern(new AssignedPatternNo4());
        RegisterOpeningPattern(new AssignedPatternNo5());
        RegisterOpeningPattern(new AssignedPatternNo6());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 0 Enter");
    }

    protected override void RegisterForcedPattern()
    {
        forcedPatterns.Add(
            new ForcedPatternRule(
                () => ai.bossStats.GetBossHPByLine() <= 120,
                new AssignedPatternNo6(),
                true
            )
        );
    }
}