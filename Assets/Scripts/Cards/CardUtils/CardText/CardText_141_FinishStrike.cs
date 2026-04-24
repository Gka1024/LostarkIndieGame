using System.Collections.Generic;
using UnityEngine;

public class CardText_TwoHSword_FinishStrike : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_TwoHSword_FinishStrike cardStat = cardStats as CardStats_TwoHSword_FinishStrike;

        if (cardStat != null)
        {
            baseValues["opt2_skill_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt2_skill_damage).ToString();
            baseValues["opt3_skill_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt3_skill_damage).ToString();

        }

        return baseValues;
    }
}
