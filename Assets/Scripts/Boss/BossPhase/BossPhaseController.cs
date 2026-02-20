using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [SerializeField] private BossStats bossStats;
    [SerializeField] private BossAI bossAI;

    private const int Phase1HP = 130; // 조우 ~ 전멸기~ 지형파괴
    private const int Phase2HP = 85; // 지형파괴 ~ 버러지
    private const int Phase3HP = 30; // 유령
    private const int Phase4HP = 30; // 유령

    private BossPatternPhase currentPhase;
    private BossPhase bossPhase;

    private List<ForcedPatternRule> forcedRules = new();
    private Queue<BossPattern> forcedPatterns = new();

    public void Initialize()
    {
        Debug.Log("BossPhaseController Initialize");
        RegisterAllGlobalPattern();
    }

    public BossPattern GetNextPattern()
    {
        EvaluateGlobalPatterns();
        PhaseCheck();

        if (currentPhase == null)
        {
            Debug.Log("페이즈가 설정되지 않았습니다.");
        }

        Debug.Log($"CurrentPhase : {currentPhase}");

        if (forcedPatterns.Count > 0)
        {
            return forcedPatterns.Dequeue();
        }

        BossPattern pattern = currentPhase.GetNextPattern();
        Debug.Log($"Pattern : {pattern}");
        return pattern;
    }

    public void OnTurnStart()
    {
        PhaseCheck();
    }

    private void PhaseCheck()
    {
        if (bossPhase != GetPhaseByHP())
        {
            bossPhase = GetPhaseByHP();
            ChangePatternPhase(GetPatternPhaseByHP(bossPhase));
        }
        else
        {
            Debug.Log("아직 페이즈가 변화하지 않았습니다.");
        }
    }

    private void ChangePatternPhase(BossPatternPhase newPhase)
    {
        if (currentPhase != newPhase)
        {
            currentPhase?.OnExit();
            currentPhase = newPhase;
            currentPhase.Init(bossAI);
            currentPhase.OnEnter();
        }
    }

    private BossPatternPhase GetPatternPhaseByHP(BossPhase phase)
    {
        switch (phase)
        {
            case BossPhase.Phase1: return new BossPatternPhase1();
            case BossPhase.Phase2: return new BossPatternPhase2();
            case BossPhase.Phase3: return new BossPatternPhase3();
            case BossPhase.Phase4: return new BossPatternPhase4();
            default: break;
        }
        return null;
    }

    private BossPhase GetPhaseByHP()
    {
        float hp = bossStats.GetBossHPByLine();
        if (hp >= Phase1HP) return BossPhase.Phase1; // 조우 ~ 벽부수기
        if (hp >= Phase2HP) return BossPhase.Phase2; // 벽부수기 ~ 지파 
        if (hp >= Phase3HP) return BossPhase.Phase3; // 지파 ~ 사방치기 이후 유령
        if (hp >= Phase4HP) return BossPhase.Phase4; // 유령

        return BossPhase.Default;
    }

    public void RegisterAllGlobalPattern()
    {
        RegisterGlobalPattern(() => bossStats.GetBossHPByLine() <= 130, new ForcedPatternNo1(), true);
    }

    private void EvaluateGlobalPatterns()
    {
        foreach (var rule in forcedRules)
        {
            rule.Evaluate();

            var pattern = rule.TryDequeue();
            if (pattern != null)
            {
                forcedPatterns.Enqueue(pattern);
            }
        }
    }

    private void RegisterGlobalPattern(System.Func<bool> condition, BossPattern pattern, bool triggerOnce = true)
    {
        forcedRules.Add(new ForcedPatternRule(condition, pattern, triggerOnce));
    }

}

public enum BossPhase
{
    Default,
    Phase1,
    Phase2,
    Phase3,
    Phase4
}
