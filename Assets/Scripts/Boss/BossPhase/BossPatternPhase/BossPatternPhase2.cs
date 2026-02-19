using UnityEngine;

public class BossPatternPhase2 : BossPatternPhase
{ // 기둥부수기 ~ 지형파괴
    protected override void RegisterPatterns()
    {
        RegisterRegularPattern(new PatternNo9());
        RegisterRegularPattern(new AssignedPatternNo1());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterRegularPattern(new AssignedPatternNo1());
        RegisterRegularPattern(new AssignedPatternNo2());
        RegisterRegularPattern(new AssignedPatternNo3());
        RegisterRegularPattern(new AssignedPatternNo4());
        RegisterRegularPattern(new AssignedPatternNo5());
        RegisterRegularPattern(new ForcedPatternNo1());
    }

    protected override void RegisterForcedPattern()
    {
        
    }
}