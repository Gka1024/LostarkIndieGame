using UnityEngine;

public class BossPatternPhase1 : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Scribble_And_Spin());
        RegisterRegularPattern(new PatternR_Rush());
        RegisterRegularPattern(new PatternR_Cross_Attack());
        RegisterRegularPattern(new PatternR_Sector_Attack_Twice());
        RegisterRegularPattern(new PatternR_Sector_Attack_Once());
        RegisterRegularPattern(new PatternR_Scribble_And_Cross());

        //RegisterRegularPattern(new AssignedPatternNo1());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternA_WhirlWind());
        RegisterOpeningPattern(new PatternA_Rush());
        RegisterOpeningPattern(new PatternA_JumpAttack());
        RegisterOpeningPattern(new PatternA_Whirlwind_Back());
        RegisterOpeningPattern(new PatternA_SpearAttack());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 0 Enter");
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 140, new PatternA_SpearAttack(), true));
    }
}