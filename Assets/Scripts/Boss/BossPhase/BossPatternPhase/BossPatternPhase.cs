using System.Collections.Generic;
using UnityEngine;

public abstract class BossPatternPhase
{
    protected BossAI ai;

    public void Init(BossAI ai)
    {
        this.ai = ai;
        RegisterRegularPatterns();
        RegisterOpeningPatterns();
        RegisterAssignedPattern();
    }

    protected List<BossPattern> patterns = new();
    protected Queue<BossPattern> openingPatternsQueue = new();
    protected List<AssignedPatternRule> assignedPatterns = new();

    protected abstract void RegisterRegularPatterns(); // 해당 페이즈에서 통상적으로 사용하는 패턴
    protected abstract void RegisterOpeningPatterns(); // 해당 페이즈 진입 시 확정적으로 사용하는 패턴
    protected abstract void RegisterAssignedPattern(); // 해당 페이즈 내에서 특정 체력 이하시 사용하는 패턴(페이즈 변화 x)

    protected void RegisterRegularPattern(BossPattern pattern)
    {
        patterns.Add(pattern);
    }

    protected void RegisterOpeningPattern(BossPattern pattern)
    {
        openingPatternsQueue.Enqueue(pattern);
    }

    public virtual BossPattern GetNextPattern()
    {
        if (openingPatternsQueue.Count > 0)
        {
            return openingPatternsQueue.Dequeue();
        }

        return GetRandomPattern();
    }

    public virtual BossPattern GetRandomPattern()
    {
        int rand = UnityEngine.Random.Range(0, patterns.Count);

        Debug.Log($"Pattern : {patterns[rand]}");
        return patterns[rand];
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}