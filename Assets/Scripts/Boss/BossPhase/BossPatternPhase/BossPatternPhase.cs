using System.Collections.Generic;
using UnityEngine;

public abstract class BossPatternPhase
{
    protected BossAI ai;

    public void Init(BossAI ai)
    {
        this.ai = ai;
        RegisterPatterns();
        RegisterOpeningPatterns();
        RegisterForcedPattern();
    }

    protected List<BossPattern> patterns = new();
    protected Queue<BossPattern> openingPatternsQueue = new();
    protected List<ForcedPatternRule> forcedPatterns = new();

    protected abstract void RegisterPatterns();
    protected abstract void RegisterOpeningPatterns();
    protected abstract void RegisterForcedPattern();

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
        return patterns[rand];
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}