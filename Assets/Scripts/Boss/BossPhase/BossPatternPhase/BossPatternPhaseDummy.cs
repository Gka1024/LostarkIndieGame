using UnityEngine;

public class BossPatternPhaseDummy : BossPatternPhase
{   // 조우~기둥부수기
    protected override void RegisterRegularPatterns()
    {
        RegisterRegularPattern(new PatternR_Outer_Grab());
    }

    protected override void RegisterOpeningPatterns()
    {
        //RegisterOpeningPattern(new PatternR_Rush_Grab_Breath());
    }

    public override void OnEnter()
    {
        Debug.Log("Phase 0 Enter");
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
        assignedPatterns.Add(
            new AssignedPatternRule(
                () => ai.bossStats.GetBossHPByLine() <= 120,
                new PatternF_Brandish_Annihilate(),
                true
            )
        );
    }


}