using System.Collections.Generic;
using UnityEngine;

public class AreaTauntEffect : FieldEffect
{
    public GameObject tauntTarget;

    public AreaTauntEffect(List<HexTile> tiles, int duration, GameObject obj)
    {
        this.tiles = tiles;
        this.duration = duration;
        this.tauntTarget = obj;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        bool isBossinArea = HexTileManager.Instance.IsBossTile(this.tiles);

        if (isBossinArea)
        {
            GameManager.Instance.GetBoss().GetComponent<Boss>().bossController.Taunt(tauntTarget, 1);
        }

    }
}