using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItem_Placeables_CampFire : BattleItemPlaceable
{
    public int healRange = 2;
    public float healAmount = 10;

    public override void OnItemPlaced()
    {
        List<HexTile> range = HexTileManager.Instance.GetTilesWithinRange(currentTile, healRange);
        AreaHealEffect effect = new AreaHealEffect(range, healAmount, placeDuration);
        VFXManager.Instance.PlayEffect(VFXID.BattleItem_Area_Heal, currentTile, placeDuration, -1.95f);
        FieldEffectManager.Instance.AddEffect(effect);
    }
}
