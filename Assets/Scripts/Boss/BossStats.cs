using System;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    public GameManager manager;

    public Boss boss;

    public BossAI bossAI;
    public BossController bossController;
    public BossPatternHelper bossPatternHelper;
    public BossStatus bossStatus;

    public BossHPBar bossHPBar; // 보스 체력바
    public BossStaggerBar staggerBar; // 보스 무력화 바

    // ==== 보스 체력

    public const float MAX_HEALTH = 24000;
    public const float MAX_HEALTH_PHASE_2 = 6000;
    public float health = 24000; // 보스 체력

    // ==== 보스 쉴드

    public float bossShield = 0;

    // ==== 보스 카운터

    private bool isCounterReady;
    private int counterDuration;

    // ==== 보스 무력화

    public bool isStaggerAble = true;
    public const float MAX_STAGGER = 1600;
    public float staggerAmount = 1600; // 보스 무력화 수치

    public bool specialStaggerAble = false;
    public float specialStaggerAmount;

    // ==== 보스 파괴

    public bool isDestroyAble = false;
    public int destroyDuration;
    public int destroyAmount;

    // ==== 보스 방어

    public float attackBase = 20; // 보스 공격력
    public float attackCur;

    private const float k = 100f;

    public float bossGetDamageRatio = 1.0f;
    public float defenceBase = 80; // 보스 방어력
    public float defenceCur;

    // ==== 디버프


    void Start()
    {
        attackCur = attackBase;
        defenceCur = defenceBase;
        staggerAmount = MAX_STAGGER;
        bossAI = GetComponent<BossAI>();
        bossPatternHelper = bossAI.bossPatternHelper;
    }

    void Update()
    {

    }

    // 데미지 관련 로직

    public float CalculateDamage(float incomeDamage)
    {
        float defenceDamage = incomeDamage * (1 - (defenceCur / (defenceCur + k)));

        float finalDamage = bossStatus.CalculateDamageOnBuffs(defenceDamage);

        return finalDamage;
    }

    public void ReceiveDamage(BossDamageData data)
    {
        float damage = data.damage;
        bool isTrueDamage = data.isTrueDamage;
        bool isCounter = data.isCounter;

        float leftOverDamage = isTrueDamage ? damage : CalculateDamage(damage);
        leftOverDamage *= bossGetDamageRatio;

        if (isCounter || isCounterReady)
        {
            SuccessCounter();
        }

        if (bossShield > 0)
        {
            if (bossShield >= damage)
            {
                bossShield -= damage;
                leftOverDamage = 0;
            }
            else
            {
                bossPatternHelper.BossShieldBroke();
                leftOverDamage -= damage;
                bossShield = 0;
            }

            bossHPBar.UpdateShieldBar(bossShield);
        }

        if (leftOverDamage > 0)
        {
            health -= leftOverDamage;
            bossHPBar.TakeDamage(leftOverDamage);
        }

        //Debug.Log("Boss HP: " + health);
        if (health <= 0)
        {
            //Destroy(gameObject);
        }
    }

    public void SetDefenceRatio(float ratio)
    {
        bossGetDamageRatio = ratio;
    }

    public void ResetDefenceRatio()
    {
        bossGetDamageRatio = 0;
    }

    // 쉴드 관련 로직

    public void CreateShield(float shield)
    {
        bossShield += shield;
        bossHPBar.UpdateShieldBar(bossShield);
    }

    public bool CheckBossShield()
    {
        return bossShield > 0;
    }

    public void RemoveShield()
    {
        bossShield = 0;
        bossHPBar.UpdateShieldBar(bossShield);
    }

    // 카운터 관련 로직

    public void CounterReady(int duration)
    {
        isCounterReady = true;
        counterDuration = duration;
    }

    public void SuccessCounter()
    {
        bossPatternHelper.SuccessCounter(true);
    }

    private void ReduceCounterDuration()
    {
        if (counterDuration > 0)
        {
            counterDuration--;

            if (counterDuration == 0)
            {
                bossPatternHelper.SuccessCounter(false);
            }
        }
    }

    // 무력화 관련 로직

    public void GetBossStagger(float stagger)
    {
        if (isStaggerAble)
        {
            this.staggerAmount -= stagger;
            staggerBar.UpdateBossStagger();
            Debug.Log("Boss Stagger : " + this.staggerAmount);

            if (staggerAmount <= 0)
            {
                bossController.MakeBossGroggy(5);
            }
        }

        if (specialStaggerAble)
        {
            specialStaggerAmount -= stagger;
            if (specialStaggerAmount <= 0)
            {
                bossController.StaggerSuccess();
            }
        }

    }


    // 파괴 관련 로직 

    public void GetBossDestroy(int amount)
    {
        if (!isDestroyAble) return;

        destroyAmount -= amount;

        if (destroyAmount <= 0)
        {
            DestroySuccess();
        }
    }

    public void SetBossDestroyAvailable(int amount, int duration)
    {
        isDestroyAble = true;
        destroyAmount = amount;
        destroyDuration = duration;
    }

    public void SetBossDestroyUnAvailable()
    {
        isDestroyAble = false;
        destroyAmount = 0;
    }

    private void ReduceDestroyDuration()
    {
        if (destroyAmount == 0) return;

        if (destroyDuration-- == 0)
        {
            SetBossDestroyUnAvailable();
        }

    }

    private void DestroySuccess()
    {
        bossController.MakeBossGroggy(3);

        bossStatus.ReduceArmorBuff();
    }

    // 디버프 관련 로직

    public void GetDefenceDown()
    {

    }


    // 일반 로직

    public void ProceedTurn()
    {
        ReduceCounterDuration();
        ReduceDestroyDuration();
    }

    // 보스 요소 리턴 로직

    public float GetBossDefence()
    {
        return defenceCur;
    }

    public float GetBossHPByLine()
    {
        return health / 150;
    }

    // 2페이즈 (유령) 용 로직

    public void EnterPhase2()
    {
        SetBossHP(MAX_HEALTH_PHASE_2);
    }

    public void SetBossHP(float value)
    {
        health = value;
        bossHPBar.SetCurrentHealth(value);
    }


}

public class BossDamageData
{
    public float damage;
    public bool isTrueDamage = false;
    public bool isCounter = false;

    public BossDamageData(float damage, bool isTrueDamage = false, bool isCounter = false)
    {
        this.damage = damage;
        this.isTrueDamage = isTrueDamage;
        this.isCounter = isCounter;
    }
}



/*
todo : 
디버프 핸들러 만들기
파괴 수치 및 파괴 만들기
무력화 로직 구체화하기
보스 상태머신 만들기


*/