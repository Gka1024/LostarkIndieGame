using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo6 : BossPattern
{ // 돌진 패턴입니다.
    public PatternNo6()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
    }

    private HexTile targetTile;

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        (List<HexTile> result, HexTile tile) = TileRayHelper.GetRayTilesForRush(current, playerTile, 2, true);

        targetTile = tile;

        return BossPatternTurnBuilder.Create(result).SetDamage(10).Build();
    }


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}