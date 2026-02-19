using System.Collections.Generic;
using UnityEngine;

public class PatternNo1 : BossPattern
{
    // 반격 패턴

    private bool isCounterTriggered;
    private float startStagger;
    public float staggerThreshold = 30f;

    private BossPatternTurnInfo counterSuccess;
    private BossPatternTurnInfo counterFail;

    private const int WAIT_TURNS = 3;

    public PatternNo1()
    {
        // 대기 턴들
        for (int i = 0; i < WAIT_TURNS; i++)
        {
            turnGenerators.Add(MakeIdleTurn);
        }

        // 마지막 반격 턴
        turnGenerators.Add(MakeCounterTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        startStagger = ai.GetBoss().stats.staggerAmount;
        isCounterTriggered = false;

        // 성공 패턴
        counterSuccess = PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 6, true),
            (3, 18, true)
        }, damage: 40, knockbackDistance: 2);

        // 실패 패턴
        counterFail = PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 6, true),
            (2, 3, false),
            (3, 10, true),
            (3, 4, false)
        }, damage: 40, knockbackDistance: 1);
    }

    protected override void OnBeforeGenerateTurn(BossAI ai)
    {
        base.OnBeforeGenerateTurn(ai);

        if (isCounterTriggered)
        {
            isFinished = true;
            return;
        }

        float currentStagger = ai.GetBoss().stats.staggerAmount;

        // 대기 턴 중에 조건 달성하면
        if (currentTurn < WAIT_TURNS &&
            currentStagger <= startStagger - staggerThreshold)
        {
            isCounterTriggered = true;

            // 즉시 반격 턴으로 이동
            currentTurn = WAIT_TURNS;
        }
    }

    private BossPatternTurnInfo MakeCounterTurn(BossAI ai)
    {
        float currentStagger = ai.GetBoss().stats.staggerAmount;

        if (currentStagger <= startStagger - staggerThreshold)
        {
            Debug.Log("보스가 강하게 반격합니다!");
            return counterSuccess;
        }

        Debug.Log("보스가 약하게 반격합니다.");
        return counterFail;
    }

    public override void OnPatternEnd(BossAI ai)
    {
        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
    }
}
