using System.Collections;
using UnityEngine;

public class Chain_TwoHSword_RedDust : ChainSkill
{
    public override IEnumerator ExecuteChain(SkillQueueData data, bool isBossHit)
    {
        yield return base.ExecuteChain(data, isBossHit);
    }
}





