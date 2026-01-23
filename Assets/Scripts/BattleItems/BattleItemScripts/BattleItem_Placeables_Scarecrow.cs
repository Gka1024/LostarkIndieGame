using System.Collections;
using UnityEngine;

public class BattleItem_Placeables_Scarecrow : BattleItemPlaceable
{
    public override void OnturnEnds()
    {
        base.OnturnEnds();
        BossController controller = GameManager.Instance.GetBoss().GetComponent<BossController>();
        controller.Taunt(this.gameObject, duration: 5);
    }
}
