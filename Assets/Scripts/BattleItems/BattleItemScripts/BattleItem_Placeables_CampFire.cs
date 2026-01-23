using System.Collections;
using UnityEngine;

public class BattleItem_Placeables_CampFire : BattleItemPlaceable
{
    public int healRange = 3;

    public override void OnturnEnds()
    {
        base.OnturnEnds();

        GameObject player = GameManager.Instance.GetPlayer();
        HexTile playerTile = player.GetComponent<PlayerMove>().GetCurrentTile();

        if (HexTileManager.Instance.GetTileDistance(currentHextile, playerTile) <= healRange)
        {
            player.GetComponent<PlayerStats>().Heal(15);
        }

    }
}
