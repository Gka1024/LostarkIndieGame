using System.Collections.Generic;
using UnityEngine;

public class PatternA_Rush : BossPattern
{
    private HexTile targetTile;
    private bool willHitWall;

    public PatternA_Rush()
    {
        turnGenerators.Add(MakeIdleTurn); // 0
        turnGenerators.Add(MakePattern1); // 1
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    // ===============================
    // 턴 데이터 생성
    // ===============================

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        (List<HexTile> result, HexTile tile) =
            TileRayHelper.GetRayTilesForRush(current, playerTile, 2, true);

        targetTile = tile;

        willHitWall =
            targetTile.currentTileState == TileState.IsWall ||
            targetTile.currentTileState == TileState.IsPillar;

        return BossPatternBuilder.Create(result).SetDamage(10).SetBreakWalls().Build();
    }

    // ===============================
    // 턴 실행 후 처리
    // ===============================

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn != 1)
            return;

        if (targetTile == null)
        {
            Debug.LogWarning("Rush targetTile is null.");
            return;
        }

        HexTile currentTile = ai.bossController.GetCurrentTile();

        // 벽을 칠 경우, 실제 이동 위치는 벽 바로 앞 타일
        HexTile moveTile = TileDirectionHelper.Instance
            .GetFrontTile(targetTile, currentTile);

        HexTile finalTile = willHitWall ? moveTile : targetTile;

        if (finalTile == null)
        {
            Debug.LogWarning("Rush finalTile is null.");
            return;
        }

        ai.GetBoss().GetComponent<BossInteraction>().Moveto(finalTile);

        if (willHitWall)
        {
            ai.bossStatus.MakeBossGroggy(5);
            ai.bossStats.EnableDestroy(30, 5);
            EstherManager.Instance.AddEstherValue(60);
            ai.bossController.AddBuff(BossBuffFactory.CreateBuff(BuffID_Boss.BUFF_RAGE, 1, 5));
        }

        HexTile objectTile = GameManager.Instance.objectManager.IsObjectExist(currentTurnInfo.TargetTiles, TileState.IsPillar);

        Debug.Log(objectTile);

        if (objectTile != null)
        {
            GameManager.Instance.objectManager.DestroyObjectByTile(objectTile);
        }
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        //animation.PlayRush();
    }


}
