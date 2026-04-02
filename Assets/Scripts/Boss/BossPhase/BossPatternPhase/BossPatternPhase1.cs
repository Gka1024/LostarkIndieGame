using UnityEngine;

public class BossPatternPhase1 : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Swing_And_Spin());
        RegisterRegularPattern(new PatternR_Rush());
        RegisterRegularPattern(new PatternR_Cross_Attack());
        RegisterRegularPattern(new PatternR_Sector_Attack_Twice());
        RegisterRegularPattern(new PatternR_Sector_Attack_Once());
        RegisterRegularPattern(new PatternR_Swing_Cross_Attack());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternA_WhirlWind());
        RegisterOpeningPattern(new PatternA_Rush());
        RegisterOpeningPattern(new PatternA_JumpAttack());
        RegisterOpeningPattern(new PatternA_Whirlwind_Back());
        RegisterOpeningPattern(new PatternA_Rush());
        RegisterOpeningPattern(new PatternR_Swing_And_Spin());
        RegisterOpeningPattern(new PatternA_SpearAttack());
        RegisterOpeningPattern(new PatternA_Rush());
        RegisterOpeningPattern(new PatternA_Rush());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 0 Enter");
    }

    protected override void RegisterAssignedPattern()
    {
        
    }
}

/*

발탄 패턴 정리

1페이즈 : 조우~ 기둥부수기

조우패턴 : 
휠윈드
돌진
점프십자찍기
휠윈드하며 돌아가기
돌진
휘적휘적 찍고 돌리기
창꽂기
돌진
돌진

1페이즈 통상패턴

휘적휘적 찍고 돌리기
돌진
제자리 휠윈드
십자찍기 휠윈드
휘두르고 십자
부채꼴
부채꼴두번

*/
