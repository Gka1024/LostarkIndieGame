using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TutorialBossPhaseController : BossPhaseController
{
    public override BossPattern GetNextPattern()
    {
        return new PatternR_Dummy();
    }
}
