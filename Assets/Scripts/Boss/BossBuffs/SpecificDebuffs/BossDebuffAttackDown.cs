using System;

public class BossDebuffAttackDown : BossBuff
{
    public BossDebuffAttackDown(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override void OnApply(BossController boss)
    {
        base.OnApply(boss);
    }
    
}
