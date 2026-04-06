using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItem_Placeables_Scarecrow : BattleItemPlaceable
{
    public int tauntRange = 2;

    public override void OnItemPlaced()
    {
        List<HexTile> range = HexTileManager.Instance.GetTilesWithinRange(currentTile, tauntRange);
        AreaTauntEffect effect = new AreaTauntEffect(range, placeDuration, gameObject);

        FieldEffectManager.Instance.AddEffect(effect);
    }
}
