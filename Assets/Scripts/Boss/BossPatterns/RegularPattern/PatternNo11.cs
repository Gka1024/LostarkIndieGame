using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo11 : BossPattern
{ // 4번찍기 패턴입니다. 
    public PatternNo11()
    {
        totalTurns = 3;

        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
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

    private BossPatternTurnInfo MakePattern1(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
    },
     damage: 50, isDown: false);

    private BossPatternTurnInfo MakePattern2(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
    },
    damage: 60, isDown: true);

    private BossPatternTurnInfo MakePattern3(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
        (5, 4, false), (5, 4, true),
    },
    damage: 60);


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}