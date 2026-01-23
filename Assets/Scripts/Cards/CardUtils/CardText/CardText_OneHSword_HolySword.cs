using System.Collections.Generic;
using UnityEngine;

public class CardText_OneHSword_HolySword : CardText
{
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_OneHSword_HolySword cardStat = cardStats as CardStats_OneHSword_HolySword;

        if (cardStat != null)
        {
            baseValues["base_skill_damage_2"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.base_skill_damage_2).ToString();
            baseValues["base_skill_range"] = cardStat.base_skill_range.ToString();

            baseValues["opt2_damage"] = Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStat.opt2_damage).ToString();
            baseValues["opt2_turns"] = cardStat.opt2_turns.ToString();

            baseValues["opt3_damage_coef"] = cardStat.opt3_damage_coef.ToString();
        }

        return baseValues;
    }
}