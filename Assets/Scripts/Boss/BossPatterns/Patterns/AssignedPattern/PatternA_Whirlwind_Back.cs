using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternA_Whirlwind_Back : BossPattern
{ // 휠윈드 하며 돌아가기
    public PatternA_Whirlwind_Back()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeIdleTurn);
    }

    private HexTile centerTile;

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn != 0 && currentTurn != 4)
        {
            HexTile targetTile = GetNextTile(ai.bossController.GetCurrentTile());
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(targetTile);
        }
        if (currentTurn == 4)
        {
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(centerTile);
        }
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