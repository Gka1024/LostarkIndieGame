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

    private float health = MAX_HEALTH;

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
    private float staggerAmount = MAX_STAGGER;
    public bool isStaggerAble = true;

    // =========================
    //  Destroy
    // =========================

    private bool isDestroyable = false;
    private int destroyAmount;
    private int destroyDuration;

    // =========================
    // Defence
    // =========================

    private const float k = 100f;

    public float bossGetDamageRatio = 1f;
    public float defenceBase = 80f;
    private float defenceCur;

    // =========================================================
    // ================== 초기화 ================================
    // =========================================================

    private void Awake()
    {
        bossAI = GetComponent<BossAI>();
        defenceCur = defenceBase;
        staggerAmount = MAX_STAGGER;
    }

    // =========================================================
    // ================== 데미지 처리 ===========================
    // =========================================================

    public void ApplyDamageData(BossDamageData data)
    {
        ReceiveDamage(data);
        ReceiveStagger(data.stagger);
        ReceiveDestroy(data.destroy);

        if (data.isCounter)
        {
            ReceiveCounter();
        }
    }

    public float CalculateDamage(float incomeDamage)
    {
        float defenceDamage =
            incomeDamage * (1 - (defenceCur / (defenceCur + k)));

        float finalDamage =
            bossStatus.CalculateDamageOnBuffsAndDebuffs(defenceDamage);

        return finalDamage;
    }

    public void ReceiveDamage(BossDamageData data)
    {
        float damage = data.damage;

        float finalDamage = data.isTrueDamage
            ? damage
            : CalculateDamage(damage);

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

        staggerAmount -= amount;
        staggerBar.UpdateBossStagger();

        if (staggerAmount <= 0)
        {
            bossStatus.MakeBossGroggy(5);
        }
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

        destroyAmount -= amount;

        if (destroyAmount <= 0)
        {
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

    public float GetCurrentStagger() => staggerAmount;

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

    public void SetDefenceRatio(float ratio)
    {
        bossGetDamageRatio = ratio;
    }

    public void ResetDefenceRatio()
    {
        bossGetDamageRatio = 1f; // 🔥 버그 수정
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
