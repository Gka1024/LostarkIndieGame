using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo3 : BossPattern
{ // 점프찍기
    public AssignedPatternNo3()
    {
        totalTurns = 2;

        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern1);
    }

    private HexTile targetTile;

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    public override void OnPatternTurn(BossAI ai)
    {
        if (currentTurn == 2)
        {
            GameManager.Instance.GetBoss().GetComponent<BossInteraction>().Moveto(targetTile);
        }
        base.OnPatternTurn(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var playerTile = ai.bossController.GetPlayerTile();
        targetTile = playerTile;

        List<HexTile> result = new()
        {
            playerTile
        };
        result.AddRange(playerTile.neighbors);

        return new BossPatternTurnInfo(result, 10, isKnockback: true, knockbackDistance: 2, breakWalls: true);
    }


    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {
          Debug.Log("점프찍기 애니메이션 타이밍");
    }
}