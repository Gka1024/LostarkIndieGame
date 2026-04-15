using System;

public class BossDebuffStun : BossBuff
{
    public BossDebuffStun(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override void OnApply(BossController boss)
    {
        base.OnApply(boss);
        boss.Stun(Duration);
    }
}
