using System.Collections.Generic;
using UnityEngine;

public class CardText_Base : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_Base cardStat = cardStats as CardStats_Base;

        if (cardStat != null)
        {
            //baseValues["text"] = cardStat.var.ToString();
            //baseValues["text"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.var2).ToString();

        }

        return baseValues;
    }
}
