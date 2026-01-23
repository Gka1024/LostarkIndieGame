using System.Collections.Generic;
using UnityEngine;

public class CardText_OneHSword_HolyProtection : CardText
{
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_OneHSword_HolyProtection cardStat = cardStats as CardStats_OneHSword_HolyProtection;

        if (cardStat != null)
        {
            baseValues["base_shields"] = cardStat.shield_amount.ToString();
            baseValues["base_shield_turns"] = cardStat.shield_turns.ToString();

            baseValues["opt1_shield_bonus"] = cardStat.opt1_shield_bonus.ToString();
            baseValues["opt1_shield_duration"] = cardStat.opt1_shield_duration.ToString();
            baseValues["opt2_heal"] = cardStat.opt2_heal.ToString();
        }

        return baseValues;
    }
}