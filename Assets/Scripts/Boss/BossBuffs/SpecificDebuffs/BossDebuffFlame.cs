using System;

public class BossDebuffFlame : BossBuff
{
    public BossDebuffFlame(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override void OnTick(BossController boss)
    {
        base.OnTick(boss);
        boss.GetBossDamageData(new BossDamageData(Data.effectValue, isTrueDamage: true));
    }
}
