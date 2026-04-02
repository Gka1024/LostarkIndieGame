using UnityEngine;

public class BossPatternPhase4 : BossPatternPhase
{
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Whirlwind());
        RegisterRegularPattern(new PatternR_Front_Back_Front());
        RegisterRegularPattern(new PatternR_Swing_And_Spin());
        RegisterRegularPattern(new PatternR_Sector_Attack_Twice());
        RegisterRegularPattern(new PatternR_Swing_Cross_Attack());
        RegisterRegularPattern(new PatternR_Counter_Attack());

    }

    protected override void RegisterOpeningPatterns()
    {
                                                                        
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 39, new PatternF_Trash_Guys(), true)); // 잡기
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 26, new PatternF_Trash_Guys(), true)); // 잡기
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 13, new PatternF_Trash_Guys(), true)); // 잡기
    }
}