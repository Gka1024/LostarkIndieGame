using System;

public class BossArmorBuff : BossBuff
{
    public BossArmorBuff(BossBuffData data, int duration, int stack = 1) : base(data, duration, stack)
    {

    }

    public override float ModifyIncomeDamage(float damage)
    {
        return damage * (1f - (effectValue * Stack));
    }
}
