using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public Boss boss;

    public CurrentBossStatus status;

    public BossAI ai;
    public BossStats bossStats;

    public BossBuffsUI bossBuffsUI;

    private bool bossTauntValue;
    private int bossTauntDuration;

    private GameObject TauntedTarget;

    [SerializeField] private bool bossGroggyValue;
    private int bossGroggyDuration = 0;

    [SerializeField] private Dictionary<int, BossBuff> bossBuffs = new();
    [SerializeField] private Dictionary<int, BossDebuff> bossDebuffs = new();


    void Start()
    {
        AddBossBuff(BossBuffFactory.CreateBuff(101, 2, -1));
    }

    public void GetBossDebuff()
    {
        AddBossDebuff(new BossDebuff(DebuffType.AttackDown, 1, 1, 1));
    }

    // ==== 통합 보스 제어기 로직

    public void SetCurrentStatus(CurrentBossStatus status)
    {
        this.status = status;

        switch (this.status)
        {
            case CurrentBossStatus.Idle: break;
            case CurrentBossStatus.Groggy: break;
            case CurrentBossStatus.Invince: break;
        }
    }

    // ==== 보스 무력화 관련 로직


    public void MakeBossGroggy(int turns)
    {
        bossGroggyValue = true;
        bossGroggyDuration = turns;

        SetCurrentStatus(CurrentBossStatus.Groggy);
        SetBossGroggyEffect(true);
    }

    private void SetBossGroggyEffect(bool value)
    {
        boss.bossEffectController.PlayGroggyEffect(value);
    }

    public bool IsBossGroggied()
    {
        return status == CurrentBossStatus.Groggy;
    }

    private void ReduceGroggyTurns()
    {
        if (bossGroggyValue)
        {
            bossGroggyDuration--;

            if (bossGroggyDuration <= 0)
            {
                bossGroggyValue = false;
                RecoverFromGroggy();
            }
        }
    }

    private void RecoverFromGroggy()
    {
        bossGroggyValue = false;
        bossGroggyDuration = 0;

        SetCurrentStatus(CurrentBossStatus.Idle);
        SetBossGroggyEffect(false);
        ai.RecoverFromGroggy();

    }

    // ==== 보스 버프 & 디버프 관련 로직

    public Dictionary<int, BossBuff> GetBossBuffs()
    {
        return bossBuffs;
    }

    public void AddBossBuff(BossBuff buff)
    {
        int id = buff.buffID;

        if (bossBuffs.ContainsKey(id))
        {
            bossBuffs[id].stack += buff.stack;
        }
        else
        {
            bossBuffs.Add(id, buff);
        }

        AlertBuffsUpdate();
    }

    public void AddBossDebuff(BossDebuff debuff)
    {
        int id = debuff.BuffID;

        if (bossDebuffs.ContainsKey(id))
        {
            bossDebuffs[id].stack += debuff.stack;
        }
        else
        {
            bossDebuffs.Add(id, debuff);
        }

        AlertBuffsUpdate();
    }

    private void ReduceBuffDuration()
    {
        List<int> removeList = new();

        foreach (var kvp in bossBuffs)
        {

            if (kvp.Value.duration > 0)
            {
                kvp.Value.duration--;
            }

            if (kvp.Value.duration == 0)
            {
                removeList.Add(kvp.Key);
            }
        }

        foreach (int key in removeList)
        {
            bossBuffs.Remove(key);
            AlertBuffsUpdate();
        }
    }

    private void ReduceDebuffDuration()
    {
        List<int> removeList = new();

        foreach (var kvp in bossDebuffs)
        {
            kvp.Value.duration--;

            if (kvp.Value.duration <= 0)
            {
                removeList.Add(kvp.Key);
            }
        }

        foreach (int key in removeList)
        {
            bossDebuffs.Remove(key);
            AlertBuffsUpdate();
        }
    }

    public float CalculateDamageOnBuffsAndDebuffs(float damage)
    {
        return CalculateDamageOnBuffs(CalculateDamageOnDebuffs(damage));
    }

    public float CalculateDamageOnBuffs(float damage)
    {
        float value = damage;

        foreach (var buff in bossBuffs)
        {
            value = buff.Value.ModifyIncomeDamage(value);
        }

        return damage;
    }

    public float CalculateDamageOnDebuffs(float damage)
    {
        float value = damage;

        foreach (var buff in bossDebuffs)
        {
            value = buff.Value.ModifyIncomeDamage(value);
        }

        return damage;
    }

    // ==== 특정 패턴용 (2번 패턴) 버프 감소 로직

    public void ReduceArmorBuff()
    {
        Debug.Log("ReduceBuffs");
        foreach (var kvp in bossBuffs)
        {
            var buff = kvp.Value;
            if (buff.buffID == 101)
            {
                buff.stack--;

                if (buff.stack == 0)
                {
                    bossBuffs.Remove(kvp.Key);
                }
            }
        }

        AlertBuffsUpdate();
    }

    // ==== 보스 도발 관련 로직

    public void Taunt(GameObject obj, int duration)
    {
        bossTauntValue = true;
        bossTauntDuration = duration;
        TauntedTarget = obj;

        ai.SetBossTaunted();
    }

    public bool IsBossTaunted()
    {
        return status == CurrentBossStatus.Taunt;
    }

    private void ReduceTauntTurns()
    {
        if (bossTauntValue)
        {
            bossTauntDuration--;

            if (bossTauntDuration <= 0)
            {
                bossTauntValue = false;
                ai.RecoverFromTaunted();
            }
        }
    }

    // 공통 로직

    private void AlertBuffsUpdate()
    {
        bossBuffsUI.UpdateBuffs(bossBuffs, bossDebuffs);
    }

    public void OnTurnEnd()
    {
        ReduceGroggyTurns();
        ReduceTauntTurns();
        ReduceBuffDuration();
        ReduceDebuffDuration();
    }

}

public enum CurrentBossStatus
{
    Idle,
    Groggy,
    Taunt,
    Invince, // 패턴 진행중 (무적)

}