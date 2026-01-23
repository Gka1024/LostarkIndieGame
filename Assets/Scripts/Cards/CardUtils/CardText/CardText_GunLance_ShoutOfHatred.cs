using System.Collections.Generic;
using UnityEngine;

public class CardText_ShoutOfHatred : CardText
{
    protected override Dictionary<string, string> CreateCardValues()
    {
        var baseValues = base.CreateCardValues();
        CardStats_GunLance_ShoutOfHatred shoutStats = cardStats as CardStats_GunLance_ShoutOfHatred;

        if (shoutStats != null)
        {
            baseValues["taunt_turns"] = shoutStats.taunt_turns.ToString();
            
            baseValues["opt2_shield_turns"] = shoutStats.opt2_shield_turns.ToString();
            baseValues["opt2_shields"] = shoutStats.opt2_shield.ToString();
            baseValues["opt3_identity_bonus"] = shoutStats.opt3_identity_bonus.ToString();
        }

        return baseValues;
    }
}
