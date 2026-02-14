using UnityEngine;

public class BossPatternPhase4 : BossPatternPhase
{
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
        RegisterRegularPattern(new AssignedPatternNo6());
    }

    protected override void RegisterForcedPattern()
    {
        
    }
}