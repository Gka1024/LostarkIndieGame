using System;
using UnityEngine;

public class BossDebuffTaunt : BossBuff
{
    private GameObject tauntTarget;

    public BossDebuffTaunt(BossBuffData data, int duration, int stack, GameObject target) : base(data, duration, stack)
    {
        this.tauntTarget = target;
    }

    public override void OnApply(BossController boss)
    {
        base.OnApply(boss);

        if (tauntTarget == null)
        {
            Debug.LogWarning("BossDebuffTaunt : 대상이 지정되지 않았습니다. ");
            return;
        }
        boss.Taunt(tauntTarget, Duration);
    }
}
