using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo0 : BossPattern
{ // 더미 패턴입니다. 
    public PatternNo0()
    {
        totalTurns = 3;

        turnGenerators.Add(MakePattern0);
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

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = new();
        attackRange.Add(ai.bossController.GetCurrentTile());

        return new BossPatternTurnInfo(attackRange, 0);
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}