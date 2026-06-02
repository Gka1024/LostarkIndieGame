using UnityEngine;

public class BossPatternPhaseDummy : BossPatternPhase
{   // 조우~기둥부수기
    public override int PhaseNumber => 0;

    protected override void RegisterRegularPatterns()
    {
        
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternR_Dummy());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 0 Enter");
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(
            new AssignedPatternRule(
                () => ai.bossStats.GetBossHPByLine() <= 120,
                new PatternF_Brandish_Annihilate(),
                true
            )
        );
    }


}