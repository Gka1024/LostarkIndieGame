using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBuffState;

public class PlayerStats : MonoBehaviour
{
    public PlayerBuffState playerBuffState;
    public PlayerStatsUI playerStatsUI; // UI

    public PlayerAnimation anim;

    // ==== 체력 관련 변수
    public float maxHealth = 500;
    public float currentHealth;

    // ==== 마나 관련 변수
    public float maxMana = 250;
    public float currentMana;
    public float regenManaOnTurns = 20;

    // ==== 아덴 관련 변수
    public const float IDENTITY_MAX = 200;
    public float playerIdentity;
    public PlayerIdentity identityUI;
    public bool isIdentityReady;

    // ==== 공격 관련 변수
    private const float BASE_PLAYER_ATTACK = 120;
    public float playerAttack;

    // ==== 방어 관련 변수
    private float basePlayerDefence;
    public float playerDefence;

    public bool isPlayerTimeStop = false;
    public int playerTimeStopDuration;

    public bool isHidingRobeUsed = false;
    private int playerHidingRobeDuration;

    // ==== 피격이상 관련 변수
    public bool isPlayerSuperArmor = false;

    [SerializeField] private bool isPlayerDown = false;
    private int playerDownDuration;

    [SerializeField] private bool isPlayerStun = false;
    private int playerStunDuration;

    [SerializeField] private bool isPlayerSilenced = false;
    private int playerSilenceDuration;

    public void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        playerAttack = BASE_PLAYER_ATTACK;
        basePlayerDefence = playerDefence;
        playerBuffState = GetComponent<PlayerBuffState>();
    }

    // ========== 체력 및 피격 로직

    public void GetPlayerDamage(PlayerGetDamageInfo info)
    {
        if (isPlayerTimeStop) return;

        if (!info.isTrueDamage)
        {
            if (isHidingRobeUsed == true)
            {
                isHidingRobeUsed = false;
                playerHidingRobeDuration = 0;
                return;
            }
        }

        TakeDamage(info.damage);

        if (!isPlayerSuperArmor)
        {
            if (info.isKnockbackAttack)
            {
                GameManager.Instance.GetBoss().GetComponent<BossAI>().bossController.PlayerKnockBack(info.knockbackDistance);
            }

            if (info.isStunAttack)
            {
                isPlayerStun = true;
                playerStunDuration = info.stunDuration;
            }

            if (info.isDownAttack)
            {
                isPlayerDown = true;
                playerDownDuration = info.downDuration;
            }

            if (info.isSilenceAttack)
            {
                isPlayerSilenced = true;
                playerSilenceDuration = info.silenceDuration;
            }
        }
    }

    private void TakeDamage(float damage)
    {
        List<ShieldData> shields = playerBuffState.GetShieldDatas();

        if (shields.Count > 0)
        {
            shields.Sort((a, b) => a.duration.CompareTo(b.duration));

            for (int i = 0; i < shields.Count; i++)
            {
                if (damage <= 0) break;

                ShieldData shield = shields[i];
                float damageToAbsorb = Mathf.Min(damage, shield.amount);
                shield.amount -= damageToAbsorb;
                damage -= damageToAbsorb;

                if (shield.amount <= 0)
                {
                    shield.onExpire?.Invoke();
                    shields.RemoveAt(i);
                    i--;
                }
                else
                {
                    shields[i] = shield;
                }

            }
            shields.RemoveAll(shield => shield.amount <= 0);
        }

        if (damage > 0)
        {
            TakeDamageOnHealth(damage);
        }

    }

    public void TakeDamageOnHealth(float damage)
    {
        currentHealth -= damage;
        playerStatsUI.UpdateHPBar(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void ReducePlayerCCDuration()
    {
        if (isPlayerDown)
        {
            if (--playerDownDuration == 0)
            {
                isPlayerDown = false;
            }
        }

        if (isPlayerSilenced)
        {
            if (--playerSilenceDuration == 0)
            {
                isPlayerSilenced = false;
            }
        }

        if (isPlayerStun)
        {
            if (--playerStunDuration == 0)
            {
                isPlayerStun = false;
            }
        }

    }

    // ========== 회복 로직 

    public void Heal(float amount, bool isPercent = false)
    {
        currentHealth += isPercent ? maxHealth * amount * 0.01f : amount;
        currentHealth = Mathf.Max(currentHealth, maxHealth);
        playerStatsUI.UpdateHPBar(currentHealth);
    }


    private void RegenHealth()
    {
        Heal(5);
    }

    // ========== 쉴드 로직 

    public void AddShield(float percent, int duration, Action onExpire = null)
    {
        playerBuffState.AddShield(percent, duration, onExpire);
    }

    // ========== 마나 로직

    public bool IsManaEnough(float mana)
    {
        return currentMana >= mana;
    }

    public void UseMana(float mana)
    {
        currentMana -= mana;
        playerStatsUI.UpdateManaBar(currentMana);
    }

    public void RegenMana()
    {
        currentMana += regenManaOnTurns;
        currentMana += playerBuffState.GetManaRegen(regenManaOnTurns);
        playerStatsUI.UpdateManaBar(currentMana);
    }

    public void AddManaRegenBuff(float multiflier, int duration)
    {
        playerBuffState.AddManaRegenBuff(multiflier, duration);
    }

    // ==== 공격력 로직

    public void AddAttackBuff(float coefficient, float additional, int duration)
    {
        playerBuffState.AddAttackBuff(coefficient, additional, duration);
    }

    public float GetPlayerAttack()
    {
        return playerBuffState.GetPlayerAttack(playerAttack);
    }

    // ========== 아덴 로직

    public void AddPlayerIdentity(float value)
    {
        playerIdentity += value;
        identityUI.SetIdentity(playerIdentity);
        identityUI.UpdateIdentityBar();

        if (playerIdentity == IDENTITY_MAX)
        {
            isIdentityReady = true;
            identityUI.SetIdentityReady(true);
        }
    }

    public void UsePlayerIdentity()
    {
        playerIdentity = 0;
        identityUI.SetIdentity(playerIdentity);
        identityUI.UpdateIdentityBar();
        identityUI.SetIdentityReady(false);
    }


    // ========== 배틀 아이템 관련 로직

    public void UsingPotionItem(PotionType item)
    {
        switch (item)
        {
            case PotionType.TimeStop:
                playerBuffState.UsingTimeStopItem(item);
                break;
        }
    }

    public void UsingSpecialItem(SpecialType type)
    {
        playerBuffState.UsingSpecialBattleItem(type);
    }

    private void ReduceSpecialItemDuration()
    {
        ReducePlayerTimeStopDuration();
        ReducePlayerHidingRobeDuration();
    }

    private void ReducePlayerTimeStopDuration()
    {
        if (playerTimeStopDuration > 0)
        {
            playerTimeStopDuration--;
            if (playerTimeStopDuration == 0)
            {
                isPlayerTimeStop = false;
            }
        }
    }

    private void ReducePlayerHidingRobeDuration()
    {
        if (playerHidingRobeDuration > 0)
        {
            playerHidingRobeDuration--;
            if (playerHidingRobeDuration == 0)
            {
                isHidingRobeUsed = false;
            }
        }
    }
    // ========== 플레이어 상태이상 관련 로직

    public void ApplyPlayerSuperArmor(bool value = true)
    {
        isPlayerSuperArmor = value;
    }

    // ========== 플레이어 상태 확인 로직

    public bool IsPlayerDead()
    {
        return currentHealth == 0;
    }

    public bool IsImmuvable()
    {
        return isPlayerDown || isPlayerStun;
    }

    public bool GetPlayerDown()
    {
        return isPlayerDown;
    }

    public bool GetPlayerSilenced()
    {
        return isPlayerSilenced;
    }

    public bool GetPlayerStun()
    {
        return isPlayerStun;
    }

    // ========== 공통 로직

    public void KillPlayerInstantly()
    {
        Die();
    }

    private void Die()
    {
        return;
    }

    public bool IsPlayerTimeStopped()
    {
        return isPlayerTimeStop;
    }

    public void ProcessTurn()
    {
        playerBuffState.OnTurnEnd();

        ReduceSpecialItemDuration();
        RegenHealth();
        RegenMana();
        ReducePlayerCCDuration();
    }

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