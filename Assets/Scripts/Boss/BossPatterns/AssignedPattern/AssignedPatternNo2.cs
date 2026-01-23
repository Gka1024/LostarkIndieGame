using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo2 : BossPattern
{ // 돌진
    public AssignedPatternNo2()
    {
        totalTurns = 2;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
    }

    private HexTile targetTile;
    private bool isHitWall;

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    public override void OnPatternTurn(BossAI ai)
    {
        fixedPlayerTile = ai.bossController.GetPlayerTile();
        base.OnPatternTurn(ai);
    }

    public override void ExecutePattern(BossAI ai)
    {
        if (currentTurn == 2)
        {
            isHitWall = targetTile.currentTileState == TileState.IsWall;

            HexTile moveTile = TileDirectionHelper.Instance.GetFrontTile(targetTile, ai.bossController.GetCurrentTile());
            ai.GetBoss().GetComponent<BossInteraction>().Moveto(isHitWall ? moveTile : targetTile);

            if (isHitWall)
            {
                ai.bossController.MakeBossGroggy(3);
                ai.bossController.MakeBossDestroyable(3, 5);
                ai.bossStatus.AddBossBuff(BossBuffFactory.CreateBuff(102, 1, 5));
                // ai.bossStatus.AddBossDebuff();

            }
        }

        base.ExecutePattern(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        (List<HexTile> result, HexTile tile) = TileRayHelper.GetRayTilesForRush(current, playerTile, 2, true);

        targetTile = tile;

        return new BossPatternTurnInfo(result, 10, breakWalls: true);
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}