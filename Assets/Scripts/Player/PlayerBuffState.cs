using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBuffState : MonoBehaviour
{
    public PlayerStats playerStats;

    private float maxHealth;

    private List<ShieldData> shields = new();
    public float playerShield;

    private List<ManaRegenBuffData> manaRegenBuffs = new();
    private List<AttackBuffData> playerAttackBuffs = new();
    private List<SpecialBuffData> playerSpecialBuffs = new();

    private float additionalManaRegen;
    private float attackCoefficient = 1f;
    private float attackAdditional = 0f;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        maxHealth = playerStats.maxHealth;
    }

    // ==== 마나 로직

    private struct ManaRegenBuffData
    {
        public float multiflier;
        public int duration;

        public ManaRegenBuffData(float multiflier, int duration)
        {
            this.multiflier = multiflier;
            this.duration = duration;
        }
    }

    public void ReduceManaRegenBuffDuration()
    {
        for (int i = manaRegenBuffs.Count - 1; i >= 0; i--)
        {
            var buff = manaRegenBuffs[i];
            buff.duration--;

            if (buff.duration <= 0)
            {
                additionalManaRegen -= buff.multiflier;
            }
            else
            {
                manaRegenBuffs[i] = buff;
            }
        }
    }

    public void AddManaRegenBuff(float multiflier, int duration)
    {
        additionalManaRegen += multiflier;
        manaRegenBuffs.Add(new ManaRegenBuffData(multiflier, duration));
    }

    public float GetManaRegen(float mana)
    {
        return mana * additionalManaRegen * 0.01f;
    }

    // ==== 쉴드 로직

    public struct ShieldData
    {
        public float amount;
        public int duration;
        public Action onExpire;

        public ShieldData(float amount, int duration, Action onExpire)
        {
            this.amount = amount;
            this.duration = duration;
            this.onExpire = onExpire;
        }
    }

    public void AddShield(float percent, int duration, Action onExpire = null)
    {
        float shieldValue = maxHealth * (percent / 100f);
        shields.Add(new ShieldData(shieldValue, duration, onExpire));
    }

    public float GetTotalShield()
    {
        float totalShield = 0;
        foreach (var shield in shields)
        {
            totalShield += shield.amount;
        }

        return totalShield;
    }

    public bool HasPlayerShield()
    {
        return shields.Count > 0;
    }

    private void ReduceShieldDuration()
    {
        for (int i = shields.Count - 1; i >= 0; i--)
        {
            ShieldData shield = shields[i];
            shield.duration--;

            if (shield.duration <= 0)
            {
                shield.onExpire?.Invoke();
                shields.RemoveAt(i);
            }
            else
            {
                shields[i] = shield;
            }
        }
    }

    public List<ShieldData> GetShieldDatas()
    {
        return shields;
    }

    // ========== 공격력 버프 로직 

    private struct AttackBuffData
    {
        public float coefficient; // 공격력을 %로 추가할 때 사용
        public float additional; // 공격력을 깡으로 추가할 때 사용
        public int duration;

        public AttackBuffData(float coefficient, float additional, int duration)
        {
            this.coefficient = coefficient;
            this.additional = additional;
            this.duration = duration;
        }
    }

    private void ReduceAttackBuffDuration()
    {
        for (int i = playerAttackBuffs.Count - 1; i >= 0; i--)
        {
            var buff = playerAttackBuffs[i];
            buff.duration--;

            if (buff.duration <= 0)
            {
                attackCoefficient -= buff.coefficient;
                attackAdditional -= buff.additional;
                playerAttackBuffs.RemoveAt(i);
            }
            else
            {
                playerAttackBuffs[i] = buff;
            }
        }
    }

    public void AddAttackBuff(float coefficient = 0f, float additional = 0f, int duration = 0)
    {
        if (coefficient > 0)
        {
            attackCoefficient += coefficient;
        }

        if (additional > 0)
        {
            attackAdditional += additional;
        }

        playerAttackBuffs.Add(new AttackBuffData(coefficient, additional, duration));
    }

    public float GetPlayerAttack(float playerAttack)
    {
        return (playerAttack * attackCoefficient) + attackAdditional;
    }

    // ======== 특수 버프 로직

    private struct SpecialBuffData // 에스더등의 특수 버프 관련 로직(예정)
    {
        public string buffName;
        public int duration;

        public SpecialBuffData(string buffName, int duration)
        {
            this.buffName = buffName;
            this.duration = duration;
        }
    }

    private void ReduceSpecialBuffDuration()
    {
        for (int i = playerSpecialBuffs.Count - 1; i >= 0; i--)
        {
            var buff = playerSpecialBuffs[i];
            buff.duration--;

            if (buff.duration <= 0)
            {
                playerSpecialBuffs.RemoveAt(i);
            }
            else
            {
                playerSpecialBuffs[i] = buff;
            }
        }
    }

    public void AddPlayerSpecialBuff(string name, int duration)
    {
        playerSpecialBuffs.Add(new SpecialBuffData(name, duration));
    }

    public bool HasPlayerSpecialBuffs(string name)
    {
        foreach (var data in playerSpecialBuffs)
        {
            if (data.buffName == name)
            {
                return true;
            }
        }
        return false;
    }

    // ==== 배틀 아이템 로직

    public void UsingTimeStopItem(PotionType item)
    {
        PlayerTimeStop(3);
    }

    public void UsingSpecialBattleItem(SpecialType item)
    {
        switch (item)
        {
            case SpecialType.PaperAmulet:
                break;
            case SpecialType.MarchingFlag:
                break;
            case SpecialType.HidingRobe:
                break;
            default: break;

        }
    }

    public void PlayerTimeStop(int duration)
    {
        playerStats.isPlayerTimeStop = true;

        playerStats.playerTimeStopDuration = duration;
    }

    public void RemoveDebuffsByPaperAmulet()
    {

    }

    public void UseHidingRobe(int duration)
    {

    }

    public void UseMarchingFlag()
    {
        GameManager.Instance.cardList.ResetQuickMove();
    }

    public void OnTurnEnd()
    {
        ReduceShieldDuration();
        ReduceAttackBuffDuration();
        ReduceSpecialBuffDuration();
    }

}

