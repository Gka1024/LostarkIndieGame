using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Smash_4Times : BossPattern
{ // 4번찍기 패턴입니다. 
    public PatternR_Smash_4Times()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 2)
        {
            isTileFixed = true;
        }
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, playerTile, 3, 60);


        return BossPatternBuilder.Create(attackRange).SetDamage(30).Build();
    }

    HexTile fixedTile;

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();

        fixedTile = playerTile;

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, playerTile, 4, 60);

        return BossPatternBuilder.Create(attackRange).SetDamage(50).Build();
    }


    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        HexTile bossTile = ai.bossController.GetCurrentTile();

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(bossTile, fixedTile, 3, 60);

        return BossPatternBuilder.Create(attackRange).SetDamage(60).Build();
    }


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}