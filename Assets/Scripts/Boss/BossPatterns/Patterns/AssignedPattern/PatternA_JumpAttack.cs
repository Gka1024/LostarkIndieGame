using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternA_JumpAttack : BossPattern
{ // 점프찍기
    public PatternA_JumpAttack()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
    }

    private HexTile targetTile;

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 1)
        {
            ai.bossInteraction.Moveto(targetTile);
        }
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var playerTile = ai.bossController.GetPlayerTile();
        targetTile = playerTile;

        List<HexTile> result = new()
        {
            playerTile
        };
        result.AddRange(playerTile.neighbors);

        return BossPatternTurnBuilder.Create(result).SetDamage(10).SetKnockback(2).SetBreakWalls().Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        Debug.Log("점프찍기 애니메이션 타이밍");
    }
}