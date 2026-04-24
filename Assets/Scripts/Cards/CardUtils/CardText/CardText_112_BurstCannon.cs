using System.Collections.Generic;
using UnityEngine;

public class CardText_GunLance_BurstCannon : CardText
{
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_GunLance_BurstCannon burstcannonStats = cardStats as CardStats_GunLance_BurstCannon;

        if (burstcannonStats != null)
        {
            //baseValues["defence_debuff"] = burstcannonStats.tripod1_defenceDebuff.ToString();
            baseValues["opt1_skill_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * burstcannonStats.opt1_damage).ToString();
            baseValues["opt3_skill_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * burstcannonStats.opt3_damage).ToString();
        }

        return baseValues;
    }
}
