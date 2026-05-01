using System;
using UnityEngine;

public class PlayerBuffShield : PlayerBuff
{
    public float Amount;
    public Action OnExpire;
    public PlayerBuffShield(PlayerBuffData data, int duration, float amount, Action onExpire) : base(data, duration)
    {
        Amount = amount;
        OnExpire = onExpire;
    }
    public override void OnRemove(PlayerStats stat) { OnExpire?.Invoke(); }
}
