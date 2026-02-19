using System.Collections.Generic;
using UnityEngine;

public class PatternNo4 : BossPattern
{
    // 구체 생성 패턴 (원혼불)

    private const int SHIELD_DURATION = 15;
    private bool isShieldBroken;

    public PatternNo4()
    {
        for (int i = 0; i < SHIELD_DURATION; i++)
        {
            turnGenerators.Add(MakeIdleTurn);
        }

        // 마지막 턴 (실패 공격 체크)
        turnGenerators.Add(MakeFailAttackTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        ai.bossPatternHelper.SpawnGhostSphere();

        ai.bossPatternHelper.SetBossDefence(0.1f);
        ai.bossPatternHelper.MakeBossShield(3000f);

        isShieldBroken = false;
    }

    protected override void OnBeforeGenerateTurn(BossAI ai)
    {
        base.OnBeforeGenerateTurn(ai);

        // 쉴드가 깨졌으면
        // 실패 공격 턴 전에 패턴 종료 처리
        if (isShieldBroken && currentTurn >= SHIELD_DURATION)
        {
            isFinished = true;
        }
    }

    private BossPatternTurnInfo MakeFailAttackTurn(BossAI ai)
    {
        if (isShieldBroken)
        {
            // 이미 깨졌으면 아무 일도 안 함
            return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
        }

        var bossTile = ai.bossController.GetCurrentTile();
        var aroundTiles = HexTileManager.Instance.GetTilesWithinRange(bossTile, 1);

        return BossPatternTurnBuilder.Create(aroundTiles).SetDamage(50).Build();
    }

    /// <summary>
    /// 구체가 파괴되었을 때 호출
    /// </summary>
    public void ShieldBroke(BossAI ai)
    {
        if (isShieldBroken)
            return;

        isShieldBroken = true;

        ai.bossPatternHelper.ResetBossDefence();
        ai.bossPatternHelper.RemoveBossShield();
        ai.bossPatternHelper.MakeBossShield(150f);
    }

    public override void OnPatternEnd(BossAI ai)
    {
        ai.bossPatternHelper.ResetBossDefence();
        ai.bossPatternHelper.RemoveBossShield();

        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 필요 시 구현
    }
}