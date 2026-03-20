using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatternF_Destroy_Land_Down : BossPattern
{ // 지파 패턴(아래)
    public PatternF_Destroy_Land_Down()
    {
        turnGenerators.Add(MakeIdleTurn); // 점프
        turnGenerators.Add(MakePattern1); // 내려찍고 타일 부수기
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern2); // 휠윈드하면서 돌아오기
        turnGenerators.Add(MakePattern2); // 
        turnGenerators.Add(MakePattern2); // 

        // 이후 패턴은 CreatePillars_after_breakTile로 연계
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnPatternEnd(BossAI ai)
    {
        // 필요 시 정리
    }

    private HexTile centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 1)
        {
            ai.bossPatternHelper.DestroyDownTiles();
            ai.GetBoss().interaction.Moveto(ai.bossPatternHelper.GetDownBossTile());
        }

        if (3 <= currentTurn && currentTurn <= 6)
        {
            HexTile targetTile = GetNextTile(ai.bossController.GetCurrentTile());
            ai.GetBoss().interaction.Moveto(targetTile);
        }
        if (currentTurn == 6)
        {
            ai.GetBoss().interaction.Moveto(centerTile);
        }
    }

    private HexTile GetNextTile(HexTile curTile)
    {
        centerTile = HexTileManager.Instance.IsThereHexTileByCube(new Vector3Int(0, 0, 0));
        return HexTileManager.Instance.tileRayHelper.GetRayNextTile(curTile, centerTile, 2);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = ai.bossPatternHelper.GetBreakTilesDown();

        return BossPatternTurnBuilder.Create(attackRange).SetDamage(1).SetSpecial().Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile curTile = ai.bossController.GetCurrentTile();

        List<HexTile> AttackRange = HexTileManager.Instance.GetTilesWithinRange(curTile, 3);

        return BossPatternTurnBuilder.Create(AttackRange).SetDamage(30).SetKnockback(1).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}