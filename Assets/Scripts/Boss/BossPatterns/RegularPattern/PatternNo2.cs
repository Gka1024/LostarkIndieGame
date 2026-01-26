using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo2 : BossPattern
{ // 앞뒤앞 패턴입니다.
    public PatternNo2()
    {
        totalTurns = 3;

        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = true;

        base.OnStartPattern(ai);
    }

    public override void OnPatternTurn(BossAI ai)
    {
        if (currentTurn == 2)
        {
            fixedPlayerTile = HexTileManager.Instance.IsThereHexTileByCube(ai.bossController.GetPlayerTile().CubeCoord * -1);
        }

        if (currentTurn == 3)
        {
            isTileFixed = false;
        }

        base.OnPatternTurn(ai);

    }


    private BossPatternTurnInfo MakePattern1(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, true), (2, 2, false),
        (3, 3, true), (3, 3, false),
        (4, 3, true), (4, 3, false),
        (5, 3, true), (5, 3, false),
    },
    damage: 50);

    private BossPatternTurnInfo MakePattern2(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, true), (2, 2, false),
        (3, 3, true), (3, 3, false),
        (4, 3, true), (4, 3, false),
        (5, 3, true), (5, 3, false),
    },
    damage: 50);

    private BossPatternTurnInfo MakePattern3(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 2, true), (2, 2, false),
        (3, 3, true), (3, 3, false),
        (4, 3, true), (4, 3, false),
        (5, 3, true), (5, 3, false),
    },
    damage: 50, isDown: true);


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}