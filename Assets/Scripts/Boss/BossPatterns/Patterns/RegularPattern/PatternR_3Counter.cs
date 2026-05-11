using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_3Counter : BossPattern
{ // 3카운터 패턴입니다. 
    public PatternR_3Counter()
    {
        turnGenerators.Add(MakeIdleTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        var current = ai.bossController.GetCurrentTile();
        var facing = ai.bossController.GetPlayerTile();
        MakePattern1(current, facing);
    }

    public void MakePattern1(HexTile current, HexTile facing)
    {
        HashSet<HexTile> attackRangeSet = new();

        List<HexTile> AttackBase = HexTileManager.Instance.GetTilesWithinRange(current, 2);
        HexTile attackTile = HexTileManager.Instance.GetRandomTile(AttackBase);

    }

    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}