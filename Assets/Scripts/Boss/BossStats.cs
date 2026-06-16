using Unity.Mathematics;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    public BossAI bossAI;
    public BossController bossController;
    public BossStatus bossStatus;

    public BossHPBar bossHPBar;
    public BossStaggerBar staggerBar;

    // =========================
    // HP
    // =========================

    public const float MAX_HEALTH = 24000;
    public const float MAX_HEALTH_PHASE_2 = 6000;

    public float health = MAX_HEALTH;

    // =========================
    // Shield
    // =========================

    private float bossShield = 0;

    // =========================
    // Counter
    // =========================

    private bool isCounterReady;
    private int counterDuration;

    // =========================
    // Stagger
    // =========================

    public const float MAX_STAGGER = 1600;
    private float curBossStagger = MAX_STAGGER;
    public bool isStaggerAble = true;

    // =========================
    //  Destroy
    // =========================

    private bool isDestroyable = false;
    private int destroyAmount;
    private int destroyDuration;

    // =========================
    // Attack
    // =========================

    private const float BOSS_ATTACK_BASE = 1;
    public float bossAttackRatio;

    // =========================
    // Defence
    // =========================

    private const float k = 100f;

    public float bossGetDamageRatio = 1f;
    public const float BOSS_DEFENCE_BASE = 80f;
    private float bossDefence;

    // =========================================================
    // ================== 초기화 ================================
    // =========================================================

    private void Awake()
    {
        bossAI = GetComponent<BossAI>();
        bossDefence = BOSS_DEFENCE_BASE;
        bossAttackRatio = BOSS_ATTACK_BASE;
        curBossStagger = MAX_STAGGER;
        bossHPBar.Init(this);
    }

    // =========================================================
    // ================== 데미지 처리 ===========================
    // =========================================================

    public float ApplyDamageData(BossDamageData data)
    {
        float finalDamage = ReceiveDamage(data);
        ReceiveStagger(data.stagger);
        ReceiveDestroy(data.destroy);

        if (data.isCounter)
        {
            ReceiveCounter();
        }

        return finalDamage;
    }

    public float CalculateDamage(float incomeDamage)
    {
        float defenceDamage =
            incomeDamage * (1 - (bossDefence / (bossDefence + k)));

        float finalDamage =
            bossStatus.CalculateDamageOnBuffsAndDebuffs(defenceDamage);

        return finalDamage;
    }

    public float ReceiveDamage(BossDamageData data)
    {
        float damage = data.damage;

        float finalDamage = data.isTrueDamage ?
        damage : CalculateDamage(damage);

        finalDamage *= bossGetDamageRatio;

        // =========================
        // Shield 처리
        // =========================

        if (bossShield > 0)
        {
            if (bossShield >= finalDamage)
            {
                bossShield -= finalDamage;
                finalDamage = 0;
            }
            else
            {
                finalDamage -= bossShield;
                bossShield = 0;

                bossAI.NotifyShieldBroken();
            }

            bossHPBar.UpdateShieldBar(bossShield);
        }

        // =========================
        // HP 감소
        // =========================

        if (finalDamage > 0)
        {
            health -= finalDamage;
            bossHPBar.TakeDamage(finalDamage);

            if (health <= 0)
            {
                bossAI.NotifyBossDead();
            }
        }

        return finalDamage;
    }

    // =========================================================
    // ================== Shield ================================
    // =========================================================

    public void CreateShield(float shield)
    {
        bossShield += shield;
        bossHPBar.UpdateShieldBar(bossShield);
    }

    public void RemoveShield()
    {
        bossShield = 0;
        bossHPBar.UpdateShieldBar(bossShield);
    }

    public void AdjustShield(float ratio)
    {
        if (!HasShield()) return;

        bossShield *= ratio;
    }

    public bool HasShield() => bossShield > 0;

    // =========================================================
    // ================== Counter ===============================
    // =========================================================

    private void ReceiveCounter()
    {
        bossAI.NotifyCounterHit();
    }


    // =========================================================
    // ================== Stagger ===============================
    // =========================================================

    private void ReceiveStagger(float amount)
    {
        GetBossStagger(amount);
    }

    public void GetBossStagger(float amount)
    {
        if (!isStaggerAble) return;

        curBossStagger -= amount;
        staggerBar.UpdateBossStagger();

        if (curBossStagger <= 0)
        {
            bossStatus.MakeBossGroggy(10);
        }
    }

    public void RecoverStagger(float amount = MAX_STAGGER)
    {
        curBossStagger = math.max(curBossStagger + amount, MAX_STAGGER);
    }

    // =========================================================
    // ================== Destroy ===============================
    // =========================================================

    private void ReceiveDestroy(int amount)
    {
        GetBossDestroy(amount);
    }

    public void GetBossDestroy(int amount)
    {
        if (!isDestroyable)
        {
            return;
        }

        int finalAmount = bossStatus.CalculateDestructionOnBuffs(amount);

        destroyAmount -= finalAmount;

        if (destroyAmount <= 0)
        {
            isDestroyable = false;
            bossAI.NotifyDestroyResult(true);
        }
    }

    public void EnableDestroy(int amount, int duration)
    {
        isDestroyable = true;
        destroyAmount = amount;
        destroyDuration = duration;
    }

    public void DisableDestroy()
    {
        isDestroyable = false;
        destroyAmount = 0;
    }

    private void ReduceDestroyDuration()
    {
        if (!isDestroyable) return;

        destroyDuration--;

        if (destroyDuration <= 0)
        {
            bossAI.NotifyDestroyResult(false);
            DisableDestroy();
        }
    }

    public float GetCurrentStagger() => curBossStagger;

    // =========================================================
    // ================== 턴 진행 ===============================
    // =========================================================

    public void OnTurnEnd()
    {
        ReduceDestroyDuration();
    }

    // =========================================================
    // ================== 외부 접근 =============================
    // =========================================================

    public void OnAttackBuffApplied(float value)
    {
        bossAttackRatio *= value;
    }

    public void OnAttackBuffRemoved(float value)
    {
        bossAttackRatio /= value;
    }

    public void SetDefenceRatio(float ratio)
    {
        bossGetDamageRatio = ratio;
    }

    public void ResetDefenceRatio()
    {
        bossGetDamageRatio = 1f;
    }

    public void SetBossHP(float value)
    {
        health = value;
        bossHPBar.SetCurrentHealth(value);
    }

    public int GetBossHPByLine()
    {
        return (int)health / 150;
    }

    public void EnterPhase2()
    {
        health = MAX_HEALTH_PHASE_2;
    }
}

public class BossDamageData
{
    public float damage;
    public float stagger;
    public int destroy;

    public bool isTrueDamage;
    public bool isCounter;

    public BossDamageData(
        float damage,
        float stagger = 0,
        int destroy = 0,
        bool isTrueDamage = false,
        bool isCounter = false)
    {
        this.damage = damage;
        this.stagger = stagger;
        this.destroy = destroy;
        this.isTrueDamage = isTrueDamage;
        this.isCounter = isCounter;
    }
}
