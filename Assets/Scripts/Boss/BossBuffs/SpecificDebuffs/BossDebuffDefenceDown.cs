using System;

public class BossDebuffDefenceDown : BossBuff
{
    public BossDebuffDefenceDown(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override float ModifyIncomeDamage(float damage)
    {
        return damage * (1f - (effectValue * Stack));
    }
}
