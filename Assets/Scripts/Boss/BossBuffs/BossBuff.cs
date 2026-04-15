using UnityEngine;

[System.Serializable]
public class BossBuff 
{
    // 데이터 보관용
    public BossBuffData Data { get; set; }

    public int ID => Data.buffID;              // 읽기 전용 (람다식 표현)
    public BuffSide Side => Data.buffSide;    // 데이터로부터 타입 가져오기

    public int Stack { get; set; }
    public int Duration { get; set; }

    public float effectValue;

    public BossBuff(BossBuffData data, int duration, int stack = 1)
    {
        this.Data = data;
        this.effectValue = data.effectValue;
        this.Duration = duration;
        this.Stack = stack;
    }

    // --- 인터페이스 공통 메서드 구현 ---
    public virtual void OnApply(BossController boss)
    {
        Debug.Log($"{ID} 버프 적용됨");
    }

    public virtual void OnTick(BossController boss)
    {
        if (Duration > 0) Duration--;
    }

    public virtual void OnRemove(BossController boss)
    {
        Debug.Log($"{ID} 버프 제거됨");
    }

    public virtual float ModifyIncomeDamage(float damage)
    {
        return damage;
    }

    public virtual int ModifyIncomeDestruction(int destruction)
    {
        return destruction;
    }
}