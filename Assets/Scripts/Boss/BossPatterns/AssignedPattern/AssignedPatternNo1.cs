using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo1 : BossPattern
{ // 휠윈드 시작

    private bool isSpinning = false;
    public AssignedPatternNo1()
    {
        totalTurns = 5;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
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
        base.OnPatternTurn(ai);

        // 턴 3 이상이면 회전 시작
        if (currentTurn == 3)
        {
            isSpinning = true;
            ai.bossAnimation.StartSpin();  // 회전 시작 애니메이션
        }

        if (isFinished)
        {
            ai.bossAnimation.StopSpin();
            isSpinning = false;
        }
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> damageRange = HexTileManager.Instance.GetTilesWithinRange(ai.GetBoss().interaction.currentTile, 3);
        return new BossPatternTurnInfo(damageRange, 60, isKnockback: true, breakWalls: true);
    }


    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

    }
}