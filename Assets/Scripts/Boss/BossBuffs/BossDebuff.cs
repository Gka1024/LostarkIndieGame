using System;

public enum DebuffType { Unset, AttackDown, MoreDestruct, DefenceDown, LessShield, Flaming, Stunning, Taunt, }

public class BossDebuff
{
    public BossBuffData data;

    public int buffID;
    public DebuffType type;

    public int duration;
    public int stack;
    public float effectValue;

    public BossDebuff(BossBuffData data, int duration, int stack = 1)
    {
        this.data = data;
        buffID = data.buffID;
        type = data.debuffType;
        effectValue = data.effectValue;
        this.duration = duration;
        this.stack = stack;
    }

    public virtual float ModifyIncomeDamage(float value)
    {
        return value;
    }
}
