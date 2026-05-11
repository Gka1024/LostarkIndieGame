using UnityEngine;

public class BossPatternPhase3 : BossPatternPhase
{   // 지형파괴 ~ 유령
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Smash_4Times());
        RegisterRegularPattern(new PatternR_Counter_Attack());
        RegisterRegularPattern(new PatternR_Cross_Whirlwind());
        RegisterRegularPattern(new PatternR_Swing_Cross_Attack());
        RegisterRegularPattern(new PatternR_Rush());
        RegisterRegularPattern(new PatternR_Cross_Attack());
        RegisterRegularPattern(new PatternR_In_Out_Attack());
        RegisterRegularPattern(new PatternR_Front_Back_Front());
        RegisterRegularPattern(new PatternR_Sector_Attack_Once());
        RegisterRegularPattern(new PatternR_Sector_Attack_Twice());

        RegisterRegularPattern(new PatternR_Rush_Grab_Breath());
       // RegisterRegularPattern(new PatternR_Ghost_Ball());

    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternF_Destroy_Land_Down());
        RegisterOpeningPattern(new PatternF_Create_Pillars_2());
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 65, new PatternF_Trash_Guys(), true)); // 버러지
    }
}