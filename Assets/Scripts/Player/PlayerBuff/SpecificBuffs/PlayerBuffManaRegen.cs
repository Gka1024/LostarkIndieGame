using System;
using UnityEngine;

public class PlayerBuffManaRegen : PlayerBuff
{
    public float value;

    public PlayerBuffManaRegen(PlayerBuffData data, int duration, float value, int stack = 1) : base(data, duration, stack)
    {
        this.value = value;
    
    }
    
}