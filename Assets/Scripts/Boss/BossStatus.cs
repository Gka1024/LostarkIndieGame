using System;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public Boss boss;
    public BossStats bossStats;
    public BossBuffsUI bossBuffsUI;

    public CurrentBossStatus status = CurrentBossStatus.Idle;

    // ==============================
    // 이벤트 (AI가 구독)
    // ==============================

    public event Action OnGroggyApplied;
    public event Action OnGroggyRecovered;

    public event Action<GameObject> OnTauntApplied;
    public event Action OnTauntRecovered;

    // ==============================
    // Groggy
    // ==============================

    private bool bossGroggyValue;
    private int bossGroggyDuration;

    // ==============================
    // Taunt
    // ==============================

    private bool bossTauntValue;
    private int bossTauntDuration;
    private GameObject tauntedTarget;

    // ==============================
    // Buff / Debuff
    // ==============================

    private Dictionary<int, BossBuff> bossBuffs = new();
    private Dictionary<int, BossBuff> bossDebuffs = new();

    public void DebugBuffs()
    {
        Debug.Log($"버프 :  {bossBuffs.Count} || 디버프 : {bossDebuffs.Count}");
    }

    // =========================================================
    // ====================== 상태 제어 ========================
    // =========================================================

    private void SetCurrentStatus(CurrentBossStatus newStatus)
    {
        status = newStatus;
    }

    public bool IsBossGroggied()
    {
        return status == CurrentBossStatus.Groggy;
    }

    public bool IsBossTaunted()
    {
        return status == CurrentBossStatus.Taunt;
    }

    // =========================================================
    // ====================== GROGGY ===========================
    // =========================================================

    public void MakeBossGroggy(int turns)
    {
        if (bossGroggyValue)
        {
            bossGroggyDuration = Mathf.Max(bossGroggyDuration, turns);
            return;
        }

        bossGroggyValue = true;
        bossGroggyDuration = turns;

        SetCurrentStatus(CurrentBossStatus.Groggy);
        boss.bossEffectController.PlayGroggyEffect(true);

        OnGroggyApplied?.Invoke();
    }

    private void ReduceGroggyTurns()
    {
        if (!bossGroggyValue) return;

        bossGroggyDuration--;

        if (bossGroggyDuration <= 0)
        {
            RecoverFromGroggy();
        }
    }

    private void RecoverFromGroggy()
    {
        bossGroggyValue = false;
        bossGroggyDuration = 0;

        boss.bossEffectController.PlayGroggyEffect(false);
        SetCurrentStatus(CurrentBossStatus.Idle);

        if (bossStats.GetCurrentStagger() == 0) bossStats.RecoverStagger();

        OnGroggyRecovered?.Invoke();
    }

    // =========================================================
    // ====================== TAUNT ============================
    // =========================================================

    public void Taunt(GameObject target, int duration)
    {
        if (bossTauntValue)
        {
            bossTauntDuration = Mathf.Max(bossTauntDuration, duration);
            tauntedTarget = target;
            return;
        }

        bossTauntValue = true;
        bossTauntDuration = duration;
        tauntedTarget = target;

        SetCurrentStatus(CurrentBossStatus.Taunt);

        OnTauntApplied?.Invoke(target);
    }

    private void ReduceTauntTurns()
    {
        if (!bossTauntValue) return;

        bossTauntDuration--;

        if (bossTauntDuration <= 0)
        {
            RecoverFromTaunt();
        }
    }

    private void RecoverFromTaunt()
    {
        bossTauntValue = false;
        bossTauntDuration = 0;
        tauntedTarget = null;

        SetCurrentStatus(CurrentBossStatus.Idle);

        OnTauntRecovered?.Invoke();
    }

    public GameObject GetTauntTarget()
    {
        return tauntedTarget;
    }

    // =========================================================
    // ====================== BUFF =============================
    // =========================================================

    public void AddBossBuff(BossBuff buff)
    {
        int id = buff.ID;

        var buffsToChange = (buff.Side == BuffSide.Buff) ? bossBuffs : bossDebuffs;

        if (buffsToChange.ContainsKey(id))
        {
            buffsToChange[id].Stack += buff.Stack;
            buffsToChange[id].Duration += buff.Duration;

        }
        else
        {
            buffsToChange.Add(id, buff);
        }

        AlertBuffsUpdate();
    }

    public void RemoveBossBuffStack(int buffId)
    {
        // 1. 버프 리스트 확인
        if (bossBuffs.ContainsKey(buffId))
        {
            HandleStackReduction(bossBuffs, buffId);
        }
        // 2. 디버프 리스트 확인 (엘스 문이 아닌 이유는 ID가 중복될 수 없지만 확실히 하기 위해)
        else if (bossDebuffs.ContainsKey(buffId))
        {
            HandleStackReduction(bossDebuffs, buffId);
        }

        // UI 업데이트 알림
        AlertBuffsUpdate();
    }

    private void HandleStackReduction(Dictionary<int, BossBuff> dictionary, int id)
    {
        var buff = dictionary[id];

        if (buff.Stack > 1)
        {
            buff.Stack--;
            Debug.Log($"보스 버프 [{id}] 스택 감소. 현재 스택: {buff.Stack}");
        }
        else
        {
            dictionary.Remove(id);
            Debug.Log($"보스 버프 [{id}] 마지막 스택 제거됨.");
        }
    }

    private void ReduceBuffDuration()
    {
        ReduceBuffDurationDictionary(bossBuffs);
        ReduceBuffDurationDictionary(bossDebuffs);
        AlertBuffsUpdate();
    }

    private void ReduceBuffDurationDictionary(Dictionary<int, BossBuff> dictionary)
    {
        List<int> removeList = new();

        foreach (var kvp in dictionary)
        {
            if (kvp.Value.Duration > 0)
                kvp.Value.Duration--;

            if (kvp.Value.Duration == 0)
                removeList.Add(kvp.Key);
        }

        foreach (int key in removeList)
            dictionary.Remove(key);
    }

    public float CalculateDamageOnBuffsAndDebuffs(float damage)
    {
        float value = damage;

        foreach (var buff in bossBuffs)
            value = buff.Value.ModifyIncomeDamage(value);

        foreach (var debuff in bossDebuffs)
            value = debuff.Value.ModifyIncomeDamage(value);

        return value;
    }

    public int CalculateDestructionOnBuffs(int destruction)
    {
        int value = destruction;
        foreach (var debuff in bossDebuffs)
            value = debuff.Value.ModifyIncomeDestruction(value);

        return value;
    }

    public void AlertBuffsUpdate()
    {
        bossBuffsUI.UpdateBuffs(bossBuffs, bossDebuffs);
    }

    // =========================================================
    // ====================== TURN END =========================
    // =========================================================

    public void OnTurnEnd()
    {
        ReduceGroggyTurns();
        ReduceTauntTurns();
        ReduceBuffDuration();
    }
}


public enum CurrentBossStatus
{
    Idle,
    Groggy,
    Taunt,
    Invince, // 패턴 진행중 (무적)

}