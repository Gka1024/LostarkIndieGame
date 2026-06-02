using Unity.Mathematics;
using UnityEngine;

public class TutorialBossStats : BossStats
{
    public const float MAX_HEALTH_TUTORIAL = 1500f;

    void Start()
    {
        SetBossHP(1500f);
    }
}

