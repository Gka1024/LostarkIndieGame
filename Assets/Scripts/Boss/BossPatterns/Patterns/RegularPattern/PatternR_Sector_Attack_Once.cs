using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Sector_Attack_Once : BossPattern
{ // 부채꼴 한번 찍기 패턴입니다. 
    public PatternR_Sector_Attack_Once()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeIdleTurn);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossController.GetCurrentTile(), ai.bossController.GetPlayerTile(), 3, 90);

        return BossPatternBuilder.Create(attackRange).SetDamage(40f).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}