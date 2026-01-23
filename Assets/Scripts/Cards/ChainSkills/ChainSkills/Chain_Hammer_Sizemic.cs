using System.Collections;
using UnityEngine;

public class Chain_Hammer_Sizemic : ChainSkill
{
    public override void SetTripod(int index)
    {
        base.SetTripod(index);

        Debug.Log("Siezmic chain tripod Set");

        if (index == 3)
        {
            if (GetOriginalCardStats() is CardStats_Hammer_SeismicHammer stat)
            {
                chainStats.skill_damage = stat.opt3_damage_coef;
                chainStats.needToSelectTile = true;
                chainStats.isDistanceSkill = true;
                chainStats.skillDistance = 1;
                chainStats.skillDistanceRange = 1;
            }
        }
    }

    public override IEnumerator ExecuteChain(SkillQueueData data)
    {
        PlayerMove playerSC = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();
        playerSC.MoveToTile(new PlayerMoveInfo(data.mainTile));
        yield return 0;
    }

}





