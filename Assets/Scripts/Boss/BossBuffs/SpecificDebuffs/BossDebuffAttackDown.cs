using System;

public class BossDebuffAttackDown : BossBuff
{
    public BossDebuffAttackDown(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override void OnApply(BossController boss)
    {
        base.OnApply(boss);
        boss.bossStats.OnAttackBuffApplied(1 - Data.effectValue);
    }

    public override void OnRemove(BossController boss)
    {
        base.OnRemove(boss);
        boss.bossStats.OnAttackBuffRemoved(1 - Data.effectValue);
    }

}
