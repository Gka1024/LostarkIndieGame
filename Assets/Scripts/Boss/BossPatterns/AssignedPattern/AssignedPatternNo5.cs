using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo5 : BossPattern
{ // 공중에서 창 내려찍기
    public AssignedPatternNo5()
    {
        totalTurns = 5;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern0);
    }

    private HexTile centerTile;

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnPatternTurn(BossAI ai)
    {
        base.OnPatternTurn(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile randomTile = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());
        HexTile playerTile = ai.bossController.GetPlayerTile();

        List<HexTile> attackRange = new();

        attackRange.Add(randomTile);
        attackRange.AddRange(randomTile.neighbors);

        attackRange.Add(playerTile);
        attackRange.AddRange(playerTile.neighbors);

        return new BossPatternTurnInfo(attackRange, 30);

    }

    private HexTile GetNextTile(HexTile curTile)
    {
        centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        return HexTileManager.Instance.tileRayHelper.GetRayNextTile(curTile, centerTile, 2);
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {

        return;
    }
}