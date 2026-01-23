using System;

public enum DebuffType { Unset, AttackDown, MoreDestruct, DefenceDown, LessShield, Flaming, Stunning, Taunt, }

public class BossDebuff
{
    public BossBuffData data;

    public int BuffID => data.buffID;
    public DebuffType type;

    public int duration;
    public int stack;
    public float effectValue;

    public BossDebuff(DebuffType type, float effectValue, int duration, int stack = 1)
    {
        this.type = type;
        this.effectValue = effectValue;
        this.duration = duration;
        this.stack = stack;
    }

    public virtual float ModifyIncomeDamage(float value)
    {
        return value;
    }
}
