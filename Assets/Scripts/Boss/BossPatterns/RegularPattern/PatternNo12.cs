using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo12 : BossPattern
{ // 더미 패턴입니다. 
    public PatternNo12()
    {
        totalTurns = 3;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    public override void OnPatternTurn(BossAI ai)
    {
        if (currentTurn == 3) // 3번째 턴(인덱스 기준)
        {
            fixedPlayerTile = ai.bossController.GetPlayerTile(); // 3턴 끝날 때 위치 저장
        }
        base.OnPatternTurn(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = new();
        attackRange.Add(ai.bossController.GetCurrentTile());

        return new BossPatternTurnInfo(attackRange, 0);
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai) => CreatePattern(ai, new[]
    {
        (4, 24, true),
        (5, 30, true),
        (6, 36, true)
    },
    damage: 60, isKnockback: true, knockbackDistance: 2);


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}