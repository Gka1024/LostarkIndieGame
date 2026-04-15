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


    public static BossBuff CreateBuff(BuffID buffID, int stack = 1, int duration = 1)
    {
        if (!buffTable.TryGetValue((int)buffID, out var data))
        {
            Debug.LogError($"등록되지 않은 BuffID: {buffID}");
            return null;
        }

        // --- 여기서 자식 클래스를 분기하여 생성합니다 ---
        // buffID(Enum)를 기반으로 switch를 돌리는 것이 가장 깔끔합니다.
        BossBuff buff = buffID switch
        {
            // 예시: 111번 ID가 화상이라면 BurnBuff(자식)를 생성
            BuffID.DEBUFF_DEFENCEDOWN => new BossDebuffDefenceDown(data, duration, stack),

            // 예시: 222번 ID가 방어력 감소라면 DefenseBuff(자식)를 생성
            BuffID.DEBUFF_ATTACKDOWN => new BossDebuffAttackDown(data, duration, stack),

            // 특별한 로직이 필요 없는 일반 버프들은 부모 클래스로 생성
            _ => new BossBuff(data, duration, stack)
        };

        return buff;
    }
}
