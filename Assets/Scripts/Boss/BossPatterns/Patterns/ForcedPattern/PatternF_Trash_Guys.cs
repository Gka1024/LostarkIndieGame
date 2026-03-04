using System.Collections.Generic;
using UnityEngine;

public class PatternF_Trash_Guys : BossPattern
{ // 버러지 패턴
    private HexTile targetTile;

    private bool counterRequired;
    private bool counterSucceed;

    public PatternF_Trash_Guys()
    {
        turnGenerators.Add(MakeIdleTurn); // 0
        turnGenerators.Add(MakeIdleTurn);// 1
        turnGenerators.Add(MakePattern1);// 2
        turnGenerators.Add(MakeIdleTurn);// 3
        turnGenerators.Add(MakePattern1);// 4
        turnGenerators.Add(MakePattern1);// 5
        turnGenerators.Add(MakePattern2);// 5
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        counterRequired = false;
        counterSucceed = false;
    }

    public override void OnPatternEnd(BossAI ai)
    {
        // 필요 시 정리
    }

    public override BossPatternTurnInfo GenerateTurn(BossAI ai)
    {
        var info = base.GenerateTurn(ai);
        if (info == null) return null;

        if (currentTurn >= 2 && !counterRequired)
        {
            counterRequired = true;
            counterSucceed = false;

            ai.bossPatternHelper.MakeBossCounter(4); // 4턴 카운터 가능
        }

        return info;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        (List<HexTile> result, HexTile tile) =
            TileRayHelper.GetRayTilesForRush(current, playerTile, 2, true);

        targetTile = tile;

        return BossPatternTurnBuilder.Create(result).SetDamage(10).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        var attackTiles = HexTileManager.Instance.GetAllTiles();

        return BossPatternTurnBuilder.Create(attackTiles).SetDamage(1).SetSpecial().Build();
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 0 || currentTurn == 1 || currentTurn == 3)
            return;

        if (targetTile == null)
        {
            Debug.LogWarning("Rush targetTile is null.");
            return;
        }

        ai.GetBoss().interaction.Moveto(targetTile);

        if (currentTurn == 5 && counterSucceed == false)
        {

        }
    }

    public override void OnBossCounterSuccess(BossAI ai)
    {
        if (!counterRequired) return;

        counterSucceed = true;
        counterRequired = false;

        ai.bossStatus.MakeBossGroggy(3);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}