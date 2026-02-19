using System.Collections.Generic;
using UnityEngine;

public class ForcedPatternNo1 : BossPattern
{
    public ForcedPatternNo1()
    {
        turnGenerators.Add(MakePattern1); // 0
        turnGenerators.Add(MakePattern2); // 1
        turnGenerators.Add(MakePattern0); // 2
        turnGenerators.Add(MakePattern0); // 3
        turnGenerators.Add(MakePattern3); // 4
        turnGenerators.Add(MakePattern0); // 5
        turnGenerators.Add(MakePattern4); // 6
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai) =>
        PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 3, true), (3, 5, true),
            (4, 2, false), (4, 6, true),
            (5, 8, true), (5, 3, false)
        },
        damage: 40);

    private BossPatternTurnInfo MakePattern2(BossAI ai) =>
        PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 3, false), (3, 5, false),
            (4, 2, true), (4, 6, false),
            (5, 8, false), (5, 3, true)
        },
        damage: 40);

    private BossPatternTurnInfo MakePattern3(BossAI ai) =>
        PatternUtility.CreatePatternByDistance(ai, new[]
        {
            (2, 2, false), (2, 2, true),
            (3, 3, true), (3, 3, false),
            (4, 3, true), (4, 3, false),
            (5, 3, true), (5, 3, false),
            (6, 2, true), (6, 2, false)
        },
        damage: 50,
        downDuration: 3);

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        var attackTiles = HexTileManager.Instance.GetAllTiles();

        return BossPatternTurnBuilder.Create(attackTiles).SetDamage(1).SetSpecial().Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {
        // 필요 시 정리
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}