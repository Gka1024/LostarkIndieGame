using System.Collections;
using UnityEngine;

public class Chain_Hammer_Sizemic : ChainSkill
{
    public override IEnumerator ExecuteChain(SkillQueueData data, bool isBossHit)
    {
        PlayerMove playerSC = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();
        playerSC.MoveToTile(new PlayerMoveInfo(data.mainTile));
        yield return base.ExecuteChain(data, isBossHit);
    }

}





