using System.Collections.Generic;
using UnityEngine;

public class CardText_Hammer_JumpingSmash : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_Hammer_JumpingSmash cardStat = cardStats as CardStats_Hammer_JumpingSmash;

        if (cardStat != null)
        {
            baseValues["base_tile"] = cardStat.skillDistance.ToString();
            baseValues["opt3_skill_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt3_skill_damage).ToString();

        }

        return baseValues;
    }
}
