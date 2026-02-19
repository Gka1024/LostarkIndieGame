using System.Collections.Generic;
using UnityEngine;

public class AssignedPatternNo2 : BossPattern
{
    private HexTile targetTile;
    private bool willHitWall;

    public AssignedPatternNo2()
    {
        turnGenerators.Add(MakePattern0); // 0
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

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

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

        return BossPatternTurnBuilder.Create(result).SetDamage(10).SetBreakWalls().Build();
    }

    // ===============================
    // 턴 실행 후 처리
    // ===============================

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        // 🔥 마지막 턴에서만 돌진 처리
        if (currentTurn != 1)
            return;

        HexTile moveTile = TileDirectionHelper.Instance
            .GetFrontTile(targetTile, ai.bossController.GetCurrentTile());

        ai.GetBoss().GetComponent<BossInteraction>()
            .Moveto(willHitWall ? moveTile : targetTile);

        if (willHitWall)
        {
            ai.bossController.MakeBossGroggy(3);
            ai.bossController.MakeBossDestroyable(3, 5);
            ai.bossStatus.AddBossBuff(
                BossBuffFactory.CreateBuff(102, 1, 5)
            );
        }

        // 오브젝트 파괴 처리
        HexTile objectTile =
            GameManager.Instance.objectManager
            .IsObjectExist(ai.currentTurnInfo.TargetTiles, TileState.IsPillar);

        if (objectTile != null)
        {
            GameManager.Instance.objectManager
                .DestroyObjectByTile(objectTile);
        }
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        //animation.PlayRush();
    }

    
}
