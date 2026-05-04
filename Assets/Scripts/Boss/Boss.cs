using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossAI ai;
    public BossAnimation animaton;
    public BossController bossController;
    public BossStats stats;
    public BossStatus status;
    public BossInteraction interaction;
    public BossEffectController bossEffectController;
    public BossDamagePopup bossDamagePopup;

    public GameObject patterns;

    public GameObject player;

    void Awake()
    {
        stats = GetComponent<BossStats>();
        ai = GetComponent<BossAI>();
        animaton = GetComponent<BossAnimation>();
        interaction = GetComponent<BossInteraction>();
    }

    void Update()
    {

    }


}
