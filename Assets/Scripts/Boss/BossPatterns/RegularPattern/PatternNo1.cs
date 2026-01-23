using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo1 : BossPattern
{// 반격 패턴입니다.
    public PatternNo1()
    {
        totalTurns = 4;
    }

    private bool isCounterTriggered;
    private float startStagger;
    public float staggerThreshold;

    private BossPatternTurnInfo counterSuccess;
    private BossPatternTurnInfo counterFail;

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        var current = ai.bossController.GetCurrentTile();
        var facing = ai.bossController.GetPlayerTile();
        startStagger = ai.GetBoss().stats.staggerAmount;

        // 성공/실패 패턴을 사전에 생성
        counterSuccess = CreatePattern(ai, new[]
        {
            (2, 6, true),  // radius 2, 360도
            (3, 18, true)  // radius 3, 3*6 = 18 방향으로 회전
        }, damage: 40, isKnockback: true, knockbackDistance: 2);

        counterFail = CreatePattern(ai, new[]
        {
            (2, 6, true),
            (2, 3, false),
            (3, 10, true),
            (3, 4, false)
        }, damage: 40, isKnockback: true, knockbackDistance: 1);
    }

    public override void OnPatternTurn(BossAI ai)
    {
        if (isFinished) return;

        float currentStagger = ai.GetBoss().stats.staggerAmount;

        if (!isCounterTriggered && currentStagger <= startStagger - staggerThreshold)
        {
            Debug.Log($"보스가 {currentStagger} 피해를 받아 강하게 반격합니다!");
            turnInfo = counterSuccess;
            ExecutePattern(ai);
            isCounterTriggered = true;
            isFinished = true;
            return;
        }

        if (currentTurn < totalTurns)
        {
            Debug.Log($"보스가 반응을 유도 중입니다... 턴 {currentTurn + 1}");
            currentTurn++;
            return;
        }

        if (!isCounterTriggered)
        {
            Debug.Log("보스가 피해를 충분히 입지 않아 약하게 반격합니다.");
            turnInfo = counterFail;
            ExecutePattern(ai);
            isCounterTriggered = true;
            isFinished = true;
        }
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}
