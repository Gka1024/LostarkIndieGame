using UnityEngine;

public class ForcedPatternRule
{
    public System.Func<bool> Condition;
    public BossPattern Pattern { get; private set; }
    public bool TriggerOnce;

    private bool hasTriggerd = false;

    public ForcedPatternRule(System.Func<bool> condition, BossPattern pattern, bool triggerOnce = true)
    {
        this.Condition = condition;
        this.Pattern = pattern;
        this.TriggerOnce = triggerOnce;
    }

    public BossPattern TryGetPattern()
    {
        if (TriggerOnce && hasTriggerd)
        {
            return null;
        }

        if (Condition())
        {
            hasTriggerd = true;
            return Pattern;
        }

        return null;
    }
}