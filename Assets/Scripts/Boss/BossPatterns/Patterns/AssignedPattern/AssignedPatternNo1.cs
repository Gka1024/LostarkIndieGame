using System.Collections.Generic;
using UnityEngine;

public class AssignedPatternNo1 : BossPattern
{
    private bool isSpinning = false;

    private int spinStartTurn = 2; // 3번째 턴 예고
    private int spinEndTurn = 4;   // 5번째 턴까지 유지

    private BossAI cachedAI;

    public AssignedPatternNo1()
    {
        turnGenerators.Add(MakePattern0); // 0
        turnGenerators.Add(MakePattern0); // 1
        turnGenerators.Add(MakePattern1); // 2
        turnGenerators.Add(MakePattern1); // 3
        turnGenerators.Add(MakePattern1); // 4
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        cachedAI = ai;
        isSpinning = false;
    }

    public override BossPatternTurnInfo GenerateTurn(BossAI ai)
    {
        var info = base.GenerateTurn(ai);
        if (info == null) return null;

        // 🔥 예고 순간에 체크
        if (currentTurn == spinStartTurn && !isSpinning)
        {
            isSpinning = true;
            ai.bossAnimation.StartSpin();
        }

        return info;
    }

    public override void CompleteTurn()
    {
        int justFinishedTurn = currentTurn;

        base.CompleteTurn();

        // 🔥 마지막 턴이 끝났을 때
        if (justFinishedTurn == spinEndTurn && isSpinning)
        {
            isSpinning = false;
            cachedAI.bossAnimation.StopSpin();
        }
    }

    public override void OnPatternEnd(BossAI ai)
    {
        if (isSpinning)
        {
            ai.bossAnimation.StopSpin();
            isSpinning = false;
        }
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var damageRange =
            HexTileManager.Instance.GetTilesWithinRange(ai.GetBoss().interaction.currentTile, 3);

        return BossPatternTurnBuilder.Create(damageRange).SetDamage(60).SetKnockback(1).SetBreakWalls().Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        //throw new System.NotImplementedException();
    }
}