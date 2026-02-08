using System.Collections.Generic;
using UnityEngine;

public abstract class BossPatternPhase
{
    protected BossAI ai;

    public void SetAI(BossAI ai)
    {
        this.ai = ai;
        RegisterPatterns();
    }

    protected List<BossPattern> patterns = new();

    protected abstract void RegisterPatterns();

    public virtual BossPattern GetRandomPattern()
    {
        if (patterns.Count == 0)
        {

        }

        int rand = UnityEngine.Random.Range(0, patterns.Count);
        return patterns[rand];
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}