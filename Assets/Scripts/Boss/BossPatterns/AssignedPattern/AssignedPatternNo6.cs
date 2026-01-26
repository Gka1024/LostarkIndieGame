using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignedPatternNo6 : BossPattern
{ // 전멸 패턴
    public AssignedPatternNo6()
    {
        totalTurns = 5;

        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern3);
        turnGenerators.Add(MakePattern0);
        turnGenerators.Add(MakePattern4);

    }

    private HexTile centerTile;

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnPatternTurn(BossAI ai)
    {
        base.OnPatternTurn(ai);
    }



    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }


    private BossPatternTurnInfo MakePattern1(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 3, true), (3, 5, true),
        (4, 2, false), (4, 6, true),
        (5, 8, true), (5, 3, false)
    },
    damage: 40);

    private BossPatternTurnInfo MakePattern2(BossAI ai) => CreatePatternByDistance(ai, new[]
    {
        (2, 3, false), (3, 5, false),
        (4, 2, true), (4, 6, false),
        (5, 8, false), (5, 3, true)
    },
    damage: 40);

    private BossPatternTurnInfo MakePattern3(BossAI ai) => CreatePatternByDistance(ai, new[]
   {
        (2, 2, false), (2, 2, true),
        (3, 3, true), (3, 3, false),
        (4, 3, true), (4, 3, false),
        (5, 3, true), (5, 3, false),
        (6, 2, true), (6, 2, false)
    },
   damage: 50, isDown: true);  // 예시: 다운 효과 추가

    private BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        List<HexTile> attackTiles = HexTileManager.Instance.GetAllTiles();

        return new BossPatternTurnInfo(attackTiles, 1, isSpecial: true);
    }

    public override void ExecutePattern(BossAI ai)
    {
        Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        HexTile playerTile = player.move.GetCurrentTile();

        foreach (HexTile tile in turnInfo.TargetTiles)
        {
            tile.isBossAttackRange = false;
            tile.ResetColor();
            tile.BreakWalls();
        }

        if (turnInfo.IsSpecial)
        {
            if (!player.stats.playerBuffState.HasPlayerSpecialBuffs("아크투르스의 가호"))
            {
                player.stats.KillPlayerInstantly();
                return;
            }
        }

        if (!turnInfo.TargetTiles.Contains(playerTile))
            return;

       // player.stats.GetPlayerDamage(turnInfo.Damage);
        Debug.Log($"턴 {currentTurn} - 플레이어가 {turnInfo.Damage} 피해를 입습니다!");

        if (turnInfo.IsKnockback)
        {
            Debug.Log($"플레이어가 {turnInfo.KnockbackDistance}칸 넉백됩니다.");
        }

        if (turnInfo.IsGrab)
        {
            Debug.Log("플레이어가 잡혔습니다!");
        }

        if (turnInfo.IsDown)
        {
            Debug.Log("플레이어가 다운됩니다!");
        }
    }
    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

        return;
    }
}