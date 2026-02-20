using System.Collections.Generic;
using UnityEngine;

public class PatternNo4 : BossPattern
{ // 원래는 감금 후 
    private const int SHIELD_DURATION = 15;

    private bool isSphereDestroyed;
    private bool isShieldBroken;

    public PatternNo4()
    {
        for (int i = 0; i < SHIELD_DURATION; i++)
        {
            turnGenerators.Add(MakeIdleTurn);
        }

        turnGenerators.Add(MakeFailAttackTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        isSphereDestroyed = false;
        isShieldBroken = false;

        ai.bossPatternHelper.SpawnGhostSphere();

        ai.bossPatternHelper.SetBossDefence(0.1f); // 데미지 감소
        ai.bossPatternHelper.CreateBossShield(3000f);
    }

    protected override void OnBeforeGenerateTurn(BossAI ai)
    {
        base.OnBeforeGenerateTurn(ai);

        // 쉴드가 깨졌으면 패턴 종료
        if (isShieldBroken)
        {
            isFinished = true;
            return;
        }
    }

    private BossPatternTurnInfo MakeFailAttackTurn(BossAI ai)
    {
        if (isShieldBroken)
        {
            return BossPatternTurnBuilder
                .Create(new List<HexTile>())
                .SetDamage(0)
                .Build();
        }

        var bossTile = ai.bossController.GetCurrentTile();
        var aroundTiles = HexTileManager.Instance.GetTilesWithinRange(bossTile, 1);

        return BossPatternTurnBuilder
            .Create(aroundTiles)
            .SetDamage(50)
            .Build();
    }

    // 🔹 구체 파괴 시 호출
    public void OnSummonedObjectDestroyed(BossPatternHelper helper)
    {
        if (isSphereDestroyed)
            return;

        isSphereDestroyed = true;

        helper.ResetBossDefence(); // 데미지 감소 해제
    }

    // 🔹 쉴드 파괴 시 호출
    public override void OnBossShieldBroke(BossPatternHelper helper)
    {
        if (isShieldBroken)
            return;

        isShieldBroken = true;

        var ai = helper.GetBossAI();

        helper.RemoveBossShield();
        helper.ResetBossDefence();

        helper.GetStatus().MakeBossGroggy(5);
        isFinished = true;
    }

    public override void OnPatternEnd(BossAI ai)
    {
        ai.bossPatternHelper.ResetBossDefence();
        ai.bossPatternHelper.RemoveBossShield();

        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
    }
}
