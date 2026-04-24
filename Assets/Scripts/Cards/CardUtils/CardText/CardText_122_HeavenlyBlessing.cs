using System.Collections.Generic;
using UnityEngine;

public class CardText_OneHSword_HeavenlyBlessing : CardText
{
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_OneHSword_HeavenlyBlessing cardStat = cardStats as CardStats_OneHSword_HeavenlyBlessing;

        if (cardStat != null)
        {
            baseValues["base_attack_increase"] = cardStat.base_buff_attack.ToString();
            baseValues["base_buff_turns"] = cardStat.base_buff_turns.ToString();

            baseValues["opt1_turns"] = cardStat.opt1_turns.ToString();
            baseValues["opt1_mana_regen"] = cardStat.opt1_mana_regen.ToString();

            baseValues["opt2_attack_increase"] = cardStat.opt2_buff_attack.ToString();
            baseValues["opt2_increase_turns"] = cardStat.opt2_buff_turns.ToString();

            baseValues["opt3_identity_bonus"] = cardStat.opt3_identity_bonus.ToString();
        }

        return baseValues;
    }
}
