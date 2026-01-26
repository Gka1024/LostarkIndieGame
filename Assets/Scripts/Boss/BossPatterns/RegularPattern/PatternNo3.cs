using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo3 : BossPattern
{ // 침묵 후 안밖 패턴입니다.

    private bool isPatternInverted;

    public PatternNo3()
    {
        totalTurns = 4;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern4);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isPatternInverted = UnityEngine.Random.value < 0.5f;
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        // 기본 공격 범위 생성
        var pattern = CreatePatternByDistance(ai, new[]
        {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
        (5, 3, false), (5, 3, true),
        (6, 2, false), (6, 2, true),
    },
        damage: 50, isDown: true);

        // isPatternInverted가 true면 타일 범위 반전
        if (isPatternInverted)
        {
            var invertedTiles = HexTileManager.Instance.GetInvertedTiles(pattern.TargetTiles);
            // 새로운 BossPatternTurnInfo로 반환
            return new BossPatternTurnInfo(
                invertedTiles,
                pattern.Damage,
                pattern.IsDown,
                pattern.IsGrab,
                pattern.IsKnockback,
                pattern.BreakWalls,
                pattern.IsSpecial,
                pattern.KnockbackDistance
            );
        }

        // 반전이 필요 없으면 원본 반환
        return pattern;
    }


    //HexTileManager.Instance.GetInvertedTiles(attackRangeList)

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}