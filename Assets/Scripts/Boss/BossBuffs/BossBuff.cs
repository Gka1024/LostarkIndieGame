using System;

public enum BuffType { Armor }

public class BossBuff
{
    public BossBuffData data;

    public int buffID;
    public BuffType type;

    public int stack;
    public int duration;
    public float effectValue;

    public BossBuff(BossBuffData data, int duration, int stack = 0)
    {
        this.data = data;
        buffID = data.buffID;
        type = data.buffType;
        effectValue = data.effectValue;
        this.duration = duration;
        this.stack = stack;
    }

    public virtual float ModifyIncomeDamage(float damage)
    {
        return damage;
    }


}
