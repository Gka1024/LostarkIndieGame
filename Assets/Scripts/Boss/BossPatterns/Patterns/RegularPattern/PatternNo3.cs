using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo3 : BossPattern
{ // 침묵 후 안밖 패턴입니다.

    private bool isPatternInverted;

    public PatternNo3()
    {
        turnGenerators.Add(MakeIdleTurn);
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
        // 기본 공격 범위 생성
        var pattern = PatternUtility.CreatePatternByDistance(ai, new[]
        {
        (2, 2, false), (2, 2, true),
        (3, 3, false), (3, 3, true),
        (4, 3, false), (4, 3, true),
        (5, 3, false), (5, 3, true),
        (6, 2, false), (6, 2, true),
    },
        damage: 50, downDuration:3);

        // isPatternInverted가 true면 타일 범위 반전
        if (isPatternInverted)
        {
            var invertedTiles = HexTileManager.Instance.GetInvertedTiles(pattern.TargetTiles);
            // 새로운 BossPatternTurnInfo로 반환
            return BossPatternTurnBuilder.Create(invertedTiles).SetDamage(50).SetDown(3).Build();
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