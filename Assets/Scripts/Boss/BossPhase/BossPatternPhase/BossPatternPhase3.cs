using UnityEngine;

public class BossPatternPhase3 : BossPatternPhase
{   // 지형파괴 ~ 유령
    protected override void RegisterPatterns()
    {
        RegisterRegularPattern(new PatternNo9());
        RegisterRegularPattern(new PatternA_WhirlWind());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterRegularPattern(new PatternA_WhirlWind());
        RegisterRegularPattern(new PatternA_Rush());
        RegisterRegularPattern(new PatternA_JumpAttack());
        RegisterRegularPattern(new PatternA_Whirlwind_Back());
        RegisterRegularPattern(new PatternA_SpearAttack());
        RegisterRegularPattern(new PatternF_Brandish_Annihilate());
    }

    protected override void RegisterForcedPattern()
    {
        
    }
}