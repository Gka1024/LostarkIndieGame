using UnityEngine;

public class BossPatternPhase2 : BossPatternPhase
{ // 기둥부수기 ~ 지형파괴
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Smash_4Times());
        RegisterRegularPattern(new PatternR_Counter_Attack());
        RegisterRegularPattern(new PatternR_Cross_Whirlwind());
        RegisterRegularPattern(new PatternR_Swing_Cross_Attack());
        RegisterRegularPattern(new PatternR_Rush());
        RegisterRegularPattern(new PatternR_Cross_Attack());
        RegisterRegularPattern(new PatternR_Portal_Rush());
        RegisterRegularPattern(new PatternR_In_Out_Attack());
        RegisterRegularPattern(new PatternR_Front_Back_Front());
        RegisterRegularPattern(new PatternR_Sector_Attack_Once());
        RegisterRegularPattern(new PatternR_Sector_Attack_Twice());
    }

    protected override void RegisterOpeningPatterns()
    {
        RegisterOpeningPattern(new PatternF_Break_All_Walls_Pillars());
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 110, new PatternF_Create_Pillars_1(), true)); // 임포스터
    }
}

/*
2페이즈 : 기둥부수기 ~ 지형파괴

통상패턴
4찍
반격 
십자 휠읜드
휘적 십자
3돌진
십자
휘적 십자
포탈돌진
휘적휘적 찍고 돌리기
안밖
앞뒤앞
부채꼴두번

*/
