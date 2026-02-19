using UnityEngine;

public class ForcedPatternRule
{
    public System.Func<bool> Condition;
    public BossPattern Pattern { get; private set; }
    public bool TriggerOnce;

    private bool hasTriggered = false;
    private bool isReserved = false;

    public ForcedPatternRule(System.Func<bool> condition, BossPattern pattern, bool triggerOnce = true)
    {
        this.Condition = condition;
        this.Pattern = pattern;
        this.TriggerOnce = triggerOnce;
    }

    // 조건이 만족되면 예약
    public void Evaluate()
    {
        if (TriggerOnce && hasTriggered)
            return;

        if (!isReserved && Condition())
        {
            isReserved = true;
        }
    }

    // 예약된 패턴 반환
    public BossPattern TryDequeue()
    {
        if (!isReserved)
            return null;

        isReserved = false;
        hasTriggered = true;
        return Pattern;
    }
}