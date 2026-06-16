using Unity.Mathematics;
using UnityEngine;

public class TutorialBossStats : BossStats
{
    public const float MAX_HEALTH_TUTORIAL = 1500f;

    void Start()
    {
        Debug.Log("tutorialBossStats");
        health = MAX_HEALTH_TUTORIAL;
        bossHPBar.Init(this);
    }
}

