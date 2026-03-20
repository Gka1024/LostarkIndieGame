using UnityEngine;

public class BossPatternPhaseDummy : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Cross_Attack());
        //RegisterRegularPattern(new AssignedPatternNo1());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternF_Break_All_Walls_Pillars());
        //RegisterOpeningPattern(new PatternF_Create_Pillars_1());

       // RegisterOpeningPattern(new PatternF_Destroy_Land_Down());
        
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