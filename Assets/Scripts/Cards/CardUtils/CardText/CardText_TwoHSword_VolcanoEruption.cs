using System.Collections.Generic;
using UnityEngine;

public class CardText_TwoHSword_VolcanoEruption : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_TwoHSword_VolcanoEruption cardStat = cardStats as CardStats_TwoHSword_VolcanoEruption;

        if (cardStat != null)
        {

            baseValues["base_skill_damage_2"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.base_skill_damage_2).ToString();

            baseValues["opt2_damage_coef"] = cardStat.opt2_damage_coef.ToString();

        }

        return baseValues;
    }
}
