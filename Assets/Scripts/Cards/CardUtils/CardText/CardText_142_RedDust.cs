using System.Collections.Generic;
using UnityEngine;

public class CardText_TwoHSword_RedDust : CardText
{
    // 스킬 전용 스탯 추가 계산
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_TwoHSword_RedDust cardStat = cardStats as CardStats_TwoHSword_RedDust;

        if (cardStat != null)
        {
            baseValues["base_buff_turns"] = cardStat.base_buff_turns.ToString();
            baseValues["base_attack_increase"] = cardStat.base_buff_attack.ToString();

            baseValues["opt1_identity_bonus"] = cardStat.opt1_identity_bonus.ToString();
            baseValues["opt2_attack_increase"] = cardStat.opt2_attack_increase.ToString();
            baseValues["opt2_increase_turns"] = cardStat.opt2_increase_turns.ToString();


        }

        return baseValues;
    }
}
