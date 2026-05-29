using System.Collections.Generic;
using UnityEngine;

public class CardText_100_Common_Basic : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_Common_Basic cardStat = cardStats as CardStats_Common_Basic;

        if (cardStat != null)
        {
            baseValues["left_cooldown"] = CardList.Instance.GetSpecialMoveCooldown().ToString();
            //baseValues["text"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.var2).ToString();

        }

        return baseValues;
    }
}
