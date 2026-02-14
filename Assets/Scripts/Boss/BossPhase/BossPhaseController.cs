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

    private List<GlobalPatternRule> globalRules = new();
    private Queue<BossPattern> globalPattern = new();

    public void Initialize()
    {
        PhaseCheck();
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

        if (globalPattern.Count > 0)
        {
            return globalPattern.Dequeue();
        }

        return currentPhase.GetNextPattern();
    }

    public void OnTurnStart()
    {
        PhaseCheck();
    }

    private void PhaseCheck()
    {
        ChangePhase(GetPhaseByHP());
    }

    private void ChangePhase(BossPatternPhase newPhase)
    {
        if (currentPhase != newPhase)
        {
            currentPhase?.OnExit();
            currentPhase = newPhase;
            currentPhase.Init(bossAI);
            currentPhase.OnEnter();
        }
    }

    private BossPatternPhase GetPhaseByHP()
    {
        float hp = bossStats.GetBossHPByLine();
        if (hp >= Phase1HP) return new BossPatternPhase1(); // 조우 ~ 벽부수기
        if (hp >= Phase2HP) return new BossPatternPhase2(); // 벽부수기 ~ 지파 
        if (hp >= Phase3HP) return new BossPatternPhase3(); // 지파 ~ 사방치기후 유령
        if (hp >= Phase4HP) return new BossPatternPhase4(); // 유령
        return new BossPatternPhase4();
    }

    public void RegisterAllGlobalPattern()
    {
        RegisterGlobalPattern(() => bossStats.GetBossHPByLine() <= 130, new AssignedPatternNo6(), true);
    }

    private void EvaluateGlobalPatterns()
    {
        foreach (var rule in globalRules)
        {
            rule.Evaluate();

            var pattern = rule.TryDequeue();
            if (pattern != null)
            {
                globalPattern.Enqueue(pattern);
            }
        }
    }

    private void RegisterGlobalPattern(System.Func<bool> condition, BossPattern pattern, bool triggerOnce = true)
    {
        globalRules.Add(new GlobalPatternRule(condition, pattern, triggerOnce));
    }

}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3,
    Phase4
}
