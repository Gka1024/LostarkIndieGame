using System.Collections.Generic;
using UnityEngine;

public class CardStats_Base : CardStats
{
    public override void ApplyOption(int num)
    {
        throw new System.NotImplementedException();
    }

    public override ChainStats GetChainStats(int tripodIndex)
    {
        ChainStats original = chainPaths.Find(p => p.tripodIndex == tripodIndex)?.chainStats;

        if (original == null) return null;

        ChainStats clonedStats = Instantiate(original);

        if (tripodIndex == 2)
        {
            clonedStats.ChangeDamage(1f);
            clonedStats.skillDistanceRange++;
        }

        return clonedStats;
    }
}
