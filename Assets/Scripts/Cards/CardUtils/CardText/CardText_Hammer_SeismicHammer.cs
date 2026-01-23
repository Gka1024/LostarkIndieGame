using System.Collections.Generic;
using UnityEngine;

public class CardText_SeismicHammer : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_Hammer_SeismicHammer cardStat = cardStats as CardStats_Hammer_SeismicHammer;

        if (cardStat != null)
        {
            baseValues["opt1_damage_coef"] = cardStat.opt1_damage_coef.ToString();
            //baseValues["text"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.var2).ToString()

        }

        return baseValues;
    }
}
