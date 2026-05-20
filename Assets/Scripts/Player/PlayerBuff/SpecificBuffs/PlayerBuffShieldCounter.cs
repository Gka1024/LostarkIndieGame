using System;
using UnityEngine;

public class PlayerBuffShieldCounter : PlayerBuff
{
    public float Amount;
    public Action OnExpire;

    public float counterDamage;
    public float counterStagger;

    public bool isBossHit;

    public PlayerBuffShieldCounter(PlayerBuffData data, int duration, float amount, Action onExpire, float damage, float stagger, bool isBossHit) : base(data, duration)
    {
        Amount = amount;
        OnExpire = onExpire;
        counterDamage = damage;
        counterStagger = stagger;
        this.isBossHit = isBossHit;
    }

    public void OnGetHit(float damage)
    {
        if (isBossHit)
        {
            BossDamageData counterData = new BossDamageData(counterDamage, counterStagger);
            SkillManager.Instance.ApplyBossSkills(counterData);
        }

        Player.Instance.state.RemoveBuff(BuffID_Player.PLAYER_SKILL_BURSTCANNON_3);
        QueueManager.Instance.Clear();
    }

    public override void OnRemove(PlayerStats stat) { OnExpire?.Invoke(); }
}
