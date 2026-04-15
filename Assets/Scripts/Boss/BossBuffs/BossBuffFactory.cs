using System.Collections.Generic;
using UnityEngine;

public static class BossBuffFactory
{
    private static Dictionary<int, BossBuffData> buffTable = new();

    public static void RegisterBuff(BossBuffData data)
    {
        if (!buffTable.ContainsKey(data.buffID))
            buffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Buff ID 중복: {data.buffID}");
    }

    public static void RegisterDebuff(BossBuffData data)
    {
        if (!buffTable.ContainsKey(data.buffID))
            buffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Debuff ID 중복: {data.buffID}");
    }


    public static BossBuff CreateBuff(BuffID buffID, int stack = 1, int duration = 1, GameObject target = null)
    {
        if (!buffTable.TryGetValue((int)buffID, out var data))
        {
            Debug.LogError($"등록되지 않은 BuffID: {buffID}");
            return null;
        }

        BossBuff buff = buffID switch
        {
            BuffID.BUFF_VALTAN_ARMOR => new BossBuffValtanArmor(data, duration, stack),
            BuffID.BUFF_RAGE => new BossBuffRage(data, duration, stack),

            BuffID.DEBUFF_DEFENCEDOWN => new BossDebuffDefenceDown(data, duration, stack),
            BuffID.DEBUFF_ATTACKDOWN => new BossDebuffAttackDown(data, duration, stack),
            BuffID.DEBUFF_TAUNT => new BossDebuffTaunt(data, duration, stack, target),
            BuffID.DEBUFF_STUN => new BossDebuffStun(data, duration, stack),
            BuffID.DEBUFF_CORROSION => new BossDebuffCorrosion(data, duration, stack),
            BuffID.DEBUFF_FRAGILE => new BossDebuffFragile(data, duration, stack),
            BuffID.DEBUFF_FLAME => new BossDebuffFlame(data, duration, stack),

            _ => new BossBuff(data, duration, stack)
        };

        return buff;
    }
}