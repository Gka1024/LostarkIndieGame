using System;

public class BossDebuffCorrosion : BossBuff
{
    public BossDebuffCorrosion(BossBuffData data, int duration, int stack) : base(data, duration, stack)
    {

    }

    public override int ModifyIncomeDestruction(int destruction)
    {
        return destruction * 2;
    }
}
