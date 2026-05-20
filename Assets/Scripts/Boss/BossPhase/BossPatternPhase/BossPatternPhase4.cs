using UnityEngine;

public class BossPatternPhase4 : BossPatternPhase
{
    public override int PhaseNumber => 4;

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
        RegisterOpeningPattern(new PatternF_Boss_Ghost());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 4 Enter");
        ai.bossStats.EnterPhase2();
    }

    private int regularPatternCount = 0;

    public override BossPattern GetNextPattern()
    {
        // 1. 오프닝 패턴 우선 처리
        if (openingPatternsQueue.Count > 0)
            return openingPatternsQueue.Dequeue();


        // 3. 일반 패턴 처리 및 3회차 체크
        BossPattern nextPattern = GetRandomPattern();
        regularPatternCount++;

        if (regularPatternCount >= 3)
        {
            Debug.Log("<color=red>4페이즈 기믹: 외곽 폭발 동시 발동!</color>");
            regularPatternCount = 0; // 카운트 초기화

            // 통상 패턴과 외곽 패턴을 합친 복합 패턴 반환
            return new CompositeBossPattern(nextPattern, new PatternR_Outer_Grab());
        }

        return nextPattern;
    }

    protected override void RegisterAssignedPattern()
    {
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 39, new PatternF_Ghost_Grab(), true)); // 잡기
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 26, new PatternF_Ghost_Grab(), true)); // 잡기
        assignedPatterns.Add(new AssignedPatternRule(() => ai.bossStats.GetBossHPByLine() <= 13, new PatternF_Ghost_Grab(), true)); // 잡기
    }
}