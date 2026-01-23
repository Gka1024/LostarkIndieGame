using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo6 : BossPattern
{ // 돌진 패턴입니다.
    public PatternNo6()
    {
        totalTurns = 4;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
    }

    private HexTile targetTile;

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
        base.ExecutePattern(ai);

        if (currentTurn == 2)
        {
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(targetTile);
        }
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

        return new BossPatternTurnInfo(result, 10);
    }


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}