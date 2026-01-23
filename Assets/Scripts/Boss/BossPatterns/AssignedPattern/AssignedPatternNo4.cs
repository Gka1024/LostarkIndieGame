using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo4 : BossPattern
{ // 휠윈드 하며 돌아가기
    public AssignedPatternNo4()
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

    public override void ExecutePattern(BossAI ai)
    {
        base.ExecutePattern(ai);
        if (currentTurn != 1 && currentTurn != 5)
        {
            HexTile targetTile = GetNextTile(ai.bossController.GetCurrentTile());
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(targetTile);
        }
        if (currentTurn == 5)
        {
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(centerTile);
        }
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();

        centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        List<HexTile> AttackRange = HexTileManager.Instance.GetTilesWithinRange(curTile, 3);

        return new BossPatternTurnInfo(AttackRange, 30, isKnockback: true, knockbackDistance: 1);
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