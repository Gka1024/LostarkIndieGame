using System.Collections.Generic;
using UnityEngine;

public class PatternNo2 : BossPattern
{
    // 앞뒤앞 패턴

    public PatternNo2()
    {
        turnGenerators.Add(ai => MakePatternCommon(ai));
        turnGenerators.Add(ai => MakePatternCommon(ai));
        turnGenerators.Add(ai => MakePatternCommon(ai, applyDown: true));
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = true;   // 첫 턴은 플레이어 기준 고정
        base.OnStartPattern(ai);
    }

    /// <summary>
    /// GenerateTurn 직전에 호출되는 훅
    /// 상태 변경은 여기서 처리한다.
    /// </summary>
    protected override void OnBeforeGenerateTurn(BossAI ai)
    {
        base.OnBeforeGenerateTurn(ai);

        // currentTurn은 0부터 시작한다고 가정

        // 2번째 턴 시작 시 (index 1)
        if (currentTurn == 1)
        {
            fixedPlayerTile = HexTileManager.Instance
                .IsThereHexTileByCube(
                    ai.bossController.GetPlayerTile().CubeCoord * -1
                );

            isTileFixed = true;
        }

        // 3번째 턴 시작 시 (index 2)
        if (currentTurn == 2)
        {
            isTileFixed = false;
        }
    }

    private BossPatternTurnInfo MakePatternCommon(BossAI ai, bool applyDown = false)
    {
        var patterns = new (int direction, int count, bool clockwise)[]
        {
            (2, 2, true), (2, 2, false),
            (3, 3, true), (3, 3, false),
            (4, 3, true), (4, 3, false),
            (5, 3, true), (5, 3, false),
        };

        return PatternUtility.CreatePatternByDistance(
            ai,
            patterns,
            damage: 50,
            downDuration: applyDown ? 2 : 0
        );
    }

    public override void OnPatternEnd(BossAI ai)
    {
        isTileFixed = false;
        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 필요 시 애니메이션 처리
    }
}
