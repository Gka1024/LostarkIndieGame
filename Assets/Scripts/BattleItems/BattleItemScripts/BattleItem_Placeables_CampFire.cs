using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItem_Placeables_CampFire : BattleItemPlaceable
{
    public int healRange = 3;
    public float healAmount = 10;

    public override void OnItemPlaced()
    {
        List<HexTile> range = HexTileManager.Instance.GetTilesWithinRange(currentTile,healRange);
        AreaHealEffect effect = new AreaHealEffect(range, healAmount, placeDuration);

        FieldEffectManager.Instance.AddEffect(effect);
    }
}
