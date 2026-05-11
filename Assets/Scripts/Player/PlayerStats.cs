using System;
using Unity.VisualScripting;
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
    public bool IsGrabbed;
    public bool IsHiding;
    public bool IsSuperArmor;

    [Header("Base Stats")]
    public const float MAX_HEALTH = 500;
    public float currentHealth;
    public float regenHealth;

    public const float MAX_MANA = 250;
    public float currentMana;
    public float regenMana;

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
    public void GivePlayerDamage(PlayerGetDamageInfo info)
    {
        if (IsTimeStopped) return;

        // 위장 로브 해제 로직
        if (IsHiding && !info.isTrueDamage)
        {
            buffState.RemoveBuff(BuffID_Player.ITEM_HIDING_ROBE);
            return;
        }

        if (buffState.GetPlayerBuff(BuffID_Player.PLAYER_SKILL_BURSTCANNON_3) is PlayerBuffShieldCounter buff)
        {
            buff.OnDamaged(info.damage);
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
        {
            PlayerKnockBack(info.knockbackDistance, info.isKnockbackToDeath);
        }

        if (info.isStunAttack)
            buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.STUN, info.stunDuration));

        if (info.isDownAttack)
            buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.DOWN, info.downDuration));

        if (info.isSilenceAttack)
            buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.SILENCE, info.silenceDuration));

        if (info.isGrabAttack)
        {
            IsGrabbed = true;
        }
    }

    public void PlayerKnockBack(int KnockbackDistance, bool isKnockbackToDeath, HexTile PlayerTile = null, HexTile BossTile = null)
    {
        if (PlayerTile == null)
        {
            PlayerTile = Player.Instance.move.GetCurrentTile();
        }

        if (BossTile == null)
        {
            BossTile = GameManager.Instance.GetBoss().GetComponent<Boss>().interaction.GetCurrentTile();
        }

        if (!isKnockbackToDeath)
        {
            HexTile tile = HexTileManager.Instance.tileBackHelper.GetBackTile(PlayerTile, BossTile, KnockbackDistance);
            if (tile.currentTileState == TileState.Default)
            {
                Player.Instance.move.PlayerKnockBack(tile);
            }
        }
        else
        {
            HexTile tile = null;
            int knockbackDist = 0;

            for (int i = 1; i <= KnockbackDistance; i++)
            {
                HexTile tileTemp = HexTileManager.Instance.tileBackHelper.GetBackTile(PlayerTile, BossTile, i);
                int distanceTemp = HexTileManager.Instance.GetTileDistance(tileTemp, PlayerTile);

                if (distanceTemp == 0)
                {
                    break;
                }
                else
                {
                    tile = tileTemp;
                    knockbackDist = distanceTemp;
                }
            }

            Player.Instance.move.PlayerKnockBack(tile);

            if (knockbackDist != KnockbackDistance)
            {
                KillPlayerInstantly();
            }
        }
    }

    public bool IsPlayerCrowdControlled()
    {
        return buffState.HasPlayerCC();
    }

    public void AddShield(float amount, int duration, Action action = null)
    {
        buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_SHIELD, duration, 0, amount, action));
        Debug.Log($"CreateShield {duration} turns");
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
        Die();
    }

    public void ProcessTurn()
    {
        buffState.OnTurnEnd(); // 모든 버프 지속시간 감소 및 효과 적용
        RegenStats();
        CheckPlayerShield();
        CheckPlayerHealth();
    }

    private void RegenStats()
    {
        currentHealth = Math.Min(MAX_HEALTH, currentHealth + regenHealth);
        float manaRegen = regenMana + buffState.GetAdditionalManaRegen(regenMana);
        currentMana = Mathf.Min(MAX_MANA, currentMana + manaRegen);

        statsUI.UpdateHPBar(currentHealth);
        statsUI.UpdateManaBar(currentMana);
    }

    private void CheckPlayerShield()
    {
        if (HasPlayerShield())
        {
            statsUI.UpdateShieldBar(buffState.GetCurrentShield());
        }
        else
        {
            statsUI.UpdateShieldBar(0);
        }
    }

    private void CheckPlayerHealth()
    {
        statsUI.UpdateHPBar(currentHealth);
    }

    public float GetCurrentAttack() => buffState.GetCalculatedAttack(baseAttack);
    public float GetStaggerMultiflyer() => 1f;
    public bool IsImmovable() => IsStunned || IsDowned || IsTimeStopped;
    public bool IsPlayerGrabbed() => IsGrabbed;
    private void Die() { Debug.Log("Die"); }
}

public class PlayerGetDamageInfo
{
    public float damage;
    public bool isTrueDamage;

    public bool isKnockbackAttack;
    public int knockbackDistance;
    public bool isKnockbackToDeath;

    public bool isStunAttack;
    public int stunDuration;

    public bool isDownAttack;
    public int downDuration;

    public bool isSilenceAttack;
    public int silenceDuration;

    public bool isGrabAttack;

    public PlayerGetDamageInfo(
        float damage,
        bool isTrueDamage = false,
        bool isKnockbackAttack = false,
        int knockbackDistance = 0,
        bool isKnockbackToDeath = false,
        bool isStunAttack = false,
        int stunDuration = 0,
        bool isDownAttack = false,
        int downDuration = 0,
        bool isSilenceAttack = false,
        int silenceDuration = 0,
        bool isGrabAttack = false
        )
    {
        this.damage = damage;
        this.isTrueDamage = isTrueDamage;
        this.isKnockbackAttack = isKnockbackAttack;
        this.knockbackDistance = knockbackDistance;
        this.isKnockbackToDeath = isKnockbackToDeath;
        this.isStunAttack = isStunAttack;
        this.stunDuration = stunDuration;
        this.isDownAttack = isDownAttack;
        this.downDuration = downDuration;
        this.isSilenceAttack = isSilenceAttack;
        this.silenceDuration = silenceDuration;
        this.isGrabAttack = isGrabAttack;
    }
}