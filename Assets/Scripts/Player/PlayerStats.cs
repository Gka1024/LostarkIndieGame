using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerBuffState buffState;
    public PlayerStatsUI statsUI;
    public PlayerAnimation anim;

    [Header("Status Flags")]
    public bool IsTimeStopped;
    public bool IsStunned;
    public bool IsDowned;
    public bool IsSilenced;
    public bool IsHiding;
    public bool IsSuperArmor;

    [Header("Base Stats")]
    public const float MAX_HEALTH = 500;
    public float currentHealth;

    public const float MAX_MANA = 250;
    public float currentMana;

    public const float MAX_IDENTITY = 200;
    public float currentIdentity;

    public float baseAttack = 120;

    private void Awake()
    {
        currentHealth = MAX_HEALTH;
        currentMana = MAX_MANA;
        currentIdentity = 0;
        buffState = GetComponent<PlayerBuffState>();
        statsUI.UpdateIdentityBar(currentIdentity);
    }

    // [핵심] 모든 데미지 로직 통합
    public void GetPlayerDamage(PlayerGetDamageInfo info)
    {
        if (IsTimeStopped) return;

        // 위장 로브 해제 로직
        if (IsHiding && !info.isTrueDamage)
        {
            buffState.RemoveBuff(BuffID_Player.ITEM_HIDING_ROBE);
            return;
        }

        // 1. 실드 처리
        float remainingDamage = buffState.AbsorbDamageWithShields(info.damage);

        // 2. 체력 차감
        if (remainingDamage > 0) TakeDamage(remainingDamage);

        // 3. CC 처리 (슈퍼아머 체크)
        if (!IsSuperArmor) ApplyCC(info);
    }

    private void ApplyCC(PlayerGetDamageInfo info)
    {
        if (info.isKnockbackAttack)

            if (info.isStunAttack)
                buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.STUN, info.stunDuration));

        if (info.isDownAttack)
            buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.DOWN, info.downDuration));

        if (info.isSilenceAttack)
            buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.SILENCE, info.silenceDuration));
    }

    public bool IsPlayerCrowdControlled()
    {
        return buffState.HasPlayerCC();
    }

    public void AddShield(float amount, int duration, Action action = null)
    {
        buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_SHIELD, duration, 0, amount, action));
    }

    public void AddAttackBuff(float amount, float additional, int duration)
    {
        buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_ATTACK_UP, duration, value: amount));
    }

    public void Heal(float amount, bool ispercent = false)
    {
        float healAmount = amount;

        if (ispercent) healAmount *= MAX_HEALTH;

        currentHealth = MathF.Min(currentHealth + healAmount, MAX_HEALTH);
    }

    public void AddPlayerIdentity(float amount)
    {

    }

    public bool HasPlayerShield()
    {
        return buffState.HasPlayerBuffs(BuffID_Player.PLAYER_SHIELD);
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        statsUI.UpdateHPBar(currentHealth);
        if (currentHealth <= 0) Die();
    }

    public void KillPlayerInstantly()
    {

    }

    public void ProcessTurn()
    {
        buffState.OnTurnEnd(); // 모든 버프 지속시간 감소 및 효과 적용
        RegenStats();
        CheckPlayerShield();
    }

    private void RegenStats()
    {
        // 체력/마나 회복 로직 (buffState에서 추가 회복량 가져옴)
        float manaRegen = 20 + buffState.GetAdditionalManaRegen(20);
        currentMana = Mathf.Min(MAX_MANA, currentMana + manaRegen);
        statsUI.UpdateManaBar(currentMana);
    }

    private void CheckPlayerShield()
    {
        if(buffState.HasPlayerBuffs(BuffID_Player.PLAYER_SHIELD))
        {
            statsUI.UpdateShieldBar(buffState.GetCurrentShield());
        }
    }

    public float GetCurrentAttack() => buffState.GetCalculatedAttack(baseAttack);
    public float GetStaggerMultiflyer() => 1f;
    public bool IsImmovable() => IsStunned || IsDowned || IsTimeStopped;
    private void Die() { /* 사망 연출 */ }
}

public class PlayerGetDamageInfo
{
    public float damage;
    public bool isTrueDamage;

    public bool isKnockbackAttack;
    public int knockbackDistance;

    public bool isStunAttack;
    public int stunDuration;

    public bool isDownAttack;
    public int downDuration;

    public bool isSilenceAttack;
    public int silenceDuration;

    public PlayerGetDamageInfo(
        float damage,
        bool isTrueDamage,
        bool isKnockbackAttack = false,
        int knockbackDistance = 0,
        bool isStunAttack = false,
        int stunDuration = 0,
        bool isDownAttack = false,
        int downDuration = 0,
        bool isSilenceAttack = false,
        int silenceDuration = 0
        )
    {
        this.damage = damage;
        this.isTrueDamage = isTrueDamage;
        this.isKnockbackAttack = isKnockbackAttack;
        this.knockbackDistance = knockbackDistance;
        this.isStunAttack = isStunAttack;
        this.stunDuration = stunDuration;
        this.isDownAttack = isDownAttack;
        this.downDuration = downDuration;
        this.isSilenceAttack = isSilenceAttack;
        this.silenceDuration = silenceDuration;
    }
}