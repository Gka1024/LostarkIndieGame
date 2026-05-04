using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerBuffFactory
{
    private static Dictionary<int, PlayerBuffData> buffTable = new();

    public static void RegisterBuff(PlayerBuffData data)
    {
        if (!buffTable.ContainsKey(data.buffID))
            buffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Buff ID 중복: {data.buffID}");
    }

    public static void RegisterDebuff(PlayerBuffData data)
    {
        if (!buffTable.ContainsKey(data.buffID))
            buffTable.Add(data.buffID, data);
        else
            Debug.LogWarning($"Buff ID 중복: {data.buffID}");
    }

    public static PlayerBuff CreateBuff(BuffID_Player id, int duration, int stack = 0, float value = 0, Action callback = null, float damage = 0, float stagger = 0, bool isBossHit = false)
    {
        if (!buffTable.TryGetValue((int)id, out var data)) return null;

        return id switch
        {
            BuffID_Player.PLAYER_ATTACK_UP => new PlayerBuffAttack(data, duration, value, stack),
            BuffID_Player.PLAYER_SHIELD => new PlayerBuffShield(data, duration, value, callback),
            BuffID_Player.PLAYER_MANA_REGEN => new PlayerBuffManaRegen(data, duration, value, stack),
            BuffID_Player.PLAYER_SKILL_BURSTCANNON_3 => new PlayerBuffShieldCounter(data, duration, value, null, damage, stagger, isBossHit),
            _ => new PlayerBuff(data, duration, 1)
        };
    }
}
