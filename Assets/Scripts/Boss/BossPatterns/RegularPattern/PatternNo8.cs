using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo8 : BossPattern
{ // 부채꼴 두번 찍기 패턴입니다.
    public PatternNo8()
    {
        totalTurns = 2;

        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
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

    private BossPatternTurnInfo MakePattern1(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
    },
    damage: 50, isDown: false);

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}