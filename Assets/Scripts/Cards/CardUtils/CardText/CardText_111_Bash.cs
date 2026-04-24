using System.Collections.Generic;
using UnityEngine;

public class CardText_GunLance_Bash : CardText
{
    // Bash 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_GunLance_Bash bashStats = cardStats as CardStats_GunLance_Bash;

        if (bashStats != null)
        {
            baseValues["opt1_defence_debuff"] = bashStats.opt1_defenceDebuff.ToString();
            baseValues["opt1_turns"] = bashStats.opt1_turns.ToString();
            baseValues["opt3_identity_bonus"] = bashStats.opt3_identityBonus.ToString();
        }

        return baseValues;
    }
}
