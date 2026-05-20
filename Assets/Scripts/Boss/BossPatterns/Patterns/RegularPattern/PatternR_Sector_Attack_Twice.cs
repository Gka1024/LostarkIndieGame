using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternR_Sector_Attack_Twice : BossPattern
{ // 부채꼴 두번 찍기 패턴입니다.(미구현)
    public PatternR_Sector_Attack_Twice()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);

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