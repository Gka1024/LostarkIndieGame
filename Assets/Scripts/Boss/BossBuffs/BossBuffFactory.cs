using System.Collections.Generic;
using UnityEngine;

public static class BossBuffFactory
{
    private static Dictionary<int, BossBuffData> buffTable = new();
    private static Dictionary<int, BossBuffData> debuffTable = new();

    public static void RegisterBuff(BossBuffData data)
    {
        if (!buffTable.ContainsKey(data.buffID))
            buffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Buff ID 중복: {data.buffID}");
    }

    public static void RegisterDebuff(BossBuffData data)
    {
        if (!debuffTable.ContainsKey(data.buffID))
            debuffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Buff ID 중복: {data.buffID}");
    }

    public static BossBuff CreateBuff(BuffID buffID, int stack = 1, int duration = 1)
    {
        if (!buffTable.TryGetValue((int)buffID, out var data))
        {
            Debug.LogError($"등록되지 않은 BuffID: {buffID}");
            return null;
        }

        BossBuff buff = new BossBuff(data, duration, stack);

        return buff;
    }

    public static BossDebuff CreateDebuff(BuffID debuffID, int stack = 1, int duration = 1)
    {
        if (!debuffTable.TryGetValue((int)debuffID, out var data))
        {
            Debug.LogError($"등록되지 않은 BuffID: {debuffID}");
            return null;
        }

        BossDebuff debuff = new BossDebuff(data, duration, stack);

        return debuff;
    }


}
