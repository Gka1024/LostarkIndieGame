using UnityEngine;

public class PlayerBuffAttack : PlayerBuff
{
    private float value;

    public PlayerBuffAttack(PlayerBuffData data, int duration, float value, int stack = 1) : base(data, duration, stack)
    {
        this.value = value;
    }

    public override float ModifyAttack(float atk)
    {
        float result = atk * (1 + value * 0.01f);
        return result;
    }
}