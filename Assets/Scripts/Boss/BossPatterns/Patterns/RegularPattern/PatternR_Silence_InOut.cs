using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Silence_InOut : BossPattern
{ // 침묵 후 안밖 패턴입니다. 침묵은 없음

    private bool isPatternInverted;

    public PatternR_Silence_InOut()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern4);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isPatternInverted = UnityEngine.Random.value < 0.5f;
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        HexTile playerTile = ai.bossController.GetPlayerTile();
        HexTile bossTile = ai.bossController.GetCurrentTile();
        HexTile attackTile = TileDirectionHelper.Instance.GetFrontTile(bossTile, playerTile, 4);

        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(attackTile, 2);

        // isPatternInverted가 true면 타일 범위 반전
        if (isPatternInverted)
        {
            var invertedTiles = HexTileManager.Instance.GetInvertedTiles(attackRange);
            // 새로운 BossPatternTurnInfo로 반환
            return BossPatternBuilder.Create(invertedTiles).SetDamage(50).SetDown(3).Build();
        }

        return BossPatternBuilder.Create(attackRange).SetDamage(1).SetKnockback(1).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}