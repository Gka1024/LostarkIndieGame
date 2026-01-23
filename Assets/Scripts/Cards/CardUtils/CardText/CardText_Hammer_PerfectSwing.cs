using System.Collections.Generic;
using UnityEngine;

public class CardText_PerfectSwing : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_Hammer_PerfectSwing cardStat = cardStats as CardStats_Hammer_PerfectSwing;

        if (cardStat != null)
        {
            baseValues["opt1_turns"] = cardStat.opt1_turns.ToString();
            baseValues["opt1_damage_coef"] = cardStat.opt1_damage_coef.ToString();

            baseValues["opt3_skill_damage_1"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt3_skill_damage_1).ToString();
            baseValues["opt3_skill_damage_2"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt3_skill_damage_2).ToString();
        }

        return baseValues;
    }
}
