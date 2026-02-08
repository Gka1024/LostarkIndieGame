using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [SerializeField] private BossStats bossStats;
    [SerializeField] private BossAI bossAI;

    private const int Phase1HP = 130; // 조우
    private const int Phase2HP = 85; // 버러지들
    private const int Phase3HP = 30; // 유령

    private BossPatternPhase currentPhase;

    public void Initialize()
    {
        ChangePhase(GetPhaseByHP());
    }

    public BossPattern GetNextPattern()
    {
        return currentPhase.GetRandomPattern();
    }

    public void OnTurnStart()
    {
        ChangePhase(GetPhaseByHP());
    }

    private void ChangePhase(BossPatternPhase newPhase)
    {
        if(currentPhase != newPhase)
        {
            currentPhase?.OnExit();
            currentPhase = newPhase;
            currentPhase.SetAI(bossAI);
            currentPhase.OnEnter();
        }
    }

    private BossPatternPhase GetPhaseByHP()
    {
        float hp = bossStats.GetBossHPByLine();
        if(hp >= Phase1HP) return new BossPatternPhase2();
        if(hp >= Phase2HP) return new BossPatternPhase2();
        if(hp >= Phase3HP) return new BossPatternPhase2();
        return new BossPatternPhase4();
    }

}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3,
    Ghost
}
