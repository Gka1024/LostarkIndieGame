using UnityEngine;

public class BossPatternPhase1 : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterPatterns()
    {
        RegisterRegularPattern(new PatternNo9());
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

    protected override void RegisterForcedPattern()
    {
        forcedPatterns.Add(
            new AssignedPatternRule(
                () => ai.bossStats.GetBossHPByLine() <= 120,
                new PatternF_Brandish_Annihilate(),
                true
            )
        );
    }
}