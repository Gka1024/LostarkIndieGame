using System.Collections.Generic;
using UnityEngine;

public class PatternR_Counter_Attack : BossPattern
{
    private bool isCounterTriggered;
    private float startStagger;
    public float staggerThreshold = 30f;

    private BossPatternTurnInfo counterSuccess;
    private BossPatternTurnInfo counterFail;

    private const int WAIT_TURNS = 3;

    public PatternR_Counter_Attack()
    {
        turnGenerators.Add(MakePattern1);

        // 대기 턴들
        for (int i = 0; i < WAIT_TURNS; i++)
        {
            turnGenerators.Add(MakeIdleTurn);
        }

        // 마지막 반격 턴
        turnGenerators.Add(MakeCounterTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        startStagger = ai.GetBoss().stats.GetCurrentStagger();
        isCounterTriggered = false;

        // 성공 패턴
        counterSuccess = BossPatternCounterSucess(ai);

        // 실패 패턴
        counterFail = BossPatternCounterFail(ai);
    }

    protected override void OnBeforeGenerateTurn(BossAI ai)
    {
        base.OnBeforeGenerateTurn(ai);

        if (isCounterTriggered)
        {
            isFinished = true;
            return;
        }

        float currentStagger = ai.GetBoss().stats.GetCurrentStagger();

        // 대기 턴 중에 조건 달성하면
        if (currentTurn < WAIT_TURNS &&
            startStagger - currentStagger >= staggerThreshold)
        {
            isCounterTriggered = true;

            // 즉시 반격 턴으로 이동
            currentTurn = WAIT_TURNS;
        }
    }

    HexTile fixedTile;

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        isTileFixed = true;
        HexTile playerTile = ai.bossController.GetPlayerTile();
        fixedTile = playerTile;

        return MakeIdleTurn(ai);
    }

    private BossPatternTurnInfo MakeCounterTurn(BossAI ai)
    {
        float currentStagger = ai.GetBoss().stats.GetCurrentStagger();

        if (startStagger - currentStagger >= staggerThreshold)
        {
            Debug.Log("보스가 강하게 반격합니다!");
            return counterSuccess;
        }

        Debug.Log("보스가 약하게 반격합니다.");
        return counterFail;
    }

    private BossPatternTurnInfo BossPatternCounterFail(BossAI ai)
    {
        HexTile bossTile = ai.bossController.GetCurrentTile();
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, fixedTile, 3, 120, 60);

        return BossPatternBuilder.Create(attackRange).SetDamage(1).SetKnockback(2, true).Build();
    }

    private BossPatternTurnInfo BossPatternCounterSucess(BossAI ai)
    {
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(GetBossTile(), 5);
        return BossPatternBuilder.Create(attackRange).SetDamage(1).SetKnockback(2, true).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {
        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
    }
}
