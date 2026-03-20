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

    protected abstract void RegisterRegularPatterns();
    protected abstract void RegisterOpeningPatterns();
    protected abstract void RegisterAssignedPattern();

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
            Debug.Log($"openingPatternsQueue : {openingPatternsQueue.Count} left");
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