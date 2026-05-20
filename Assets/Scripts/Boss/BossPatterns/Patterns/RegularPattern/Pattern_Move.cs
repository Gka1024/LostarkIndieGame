using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pattern_Move : BossPattern
{ // 더미 패턴입니다. 
    public Pattern_Move()
    {
        turnGenerators.Add(MakePatternMove);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        isTileFixed = false;
    }

    private BossPatternTurnInfo MakePatternMove(BossAI ai)
    {
        HexTile bosstile = ai.bossController.GetCurrentTile();
        HexTile playertile = ai.bossController.GetPlayerTile();
        HexTile MoveTile = TileDirectionHelper.Instance.GetFrontTile(bosstile, playertile, 2);

        ai.bossInteraction.Moveto(MoveTile);

        return MakeIdleTurn(ai);
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        List<HexTile> attackRange = new();
        attackRange.Add(ai.bossController.GetCurrentTile());
        attackRange.Add(ai.bossController.GetPlayerTile()); // 플레이어를 볼때
        attackRange.Add(Player.Instance.move.GetCurrentTile()); // 플레이어를 보지 않을때

        return BossPatternBuilder.Create(attackRange).SetDamage(0).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}