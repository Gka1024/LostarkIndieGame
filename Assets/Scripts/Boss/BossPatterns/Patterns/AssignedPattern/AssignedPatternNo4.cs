using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo4 : BossPattern
{ // 휠윈드 하며 돌아가기
    public AssignedPatternNo4()
    {
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

    public override void OnAfterTurnExecuted(BossAI ai)
    {
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
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();

        centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        List<HexTile> AttackRange = HexTileManager.Instance.GetTilesWithinRange(curTile, 3);

        return BossPatternTurnBuilder.Create(AttackRange).SetDamage(30).SetKnockback(1).Build();
    }

    private HexTile GetNextTile(HexTile curTile)
    {
        centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        return HexTileManager.Instance.tileRayHelper.GetRayNextTile(curTile, centerTile, 2);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

        return;
    }
}