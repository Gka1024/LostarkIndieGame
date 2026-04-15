using System;

public class BossDebuffFragile : BossBuff
{ // 보호막 감소
    public BossDebuffFragile(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override void OnApply(BossController boss)
    {
        base.OnApply(boss);

        if (boss.bossStats.HasShield())
        {
            boss.bossStats.AdjustShield(Data.effectValue * 0.01f);
        }
    }
}
