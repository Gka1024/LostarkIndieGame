using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PatternR_Rush_Grab_Breath : BossPattern
{ // 잡고 날리기
    public PatternR_Rush_Grab_Breath()
    {
        turnGenerators.Add(MakePattern1); // 0
        turnGenerators.Add(MakePattern2); // 1
        turnGenerators.Add(MakePattern3); // 2

    }
    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn != 2)
            return;

        if (targetTile == null)
        {
            Debug.LogWarning("Rush targetTile is null.");
            return;
        }

        HexTile currentTile = ai.bossController.GetCurrentTile();

        // 벽을 칠 경우, 실제 이동 위치는 벽 바로 앞 타일
        HexTile moveTile = TileDirectionHelper.Instance
            .GetFrontTile(targetTile, currentTile);


        if (moveTile == null)
        {
            Debug.LogWarning("Rush finalTile is null.");
            return;
        }

        ai.GetBoss().GetComponent<BossInteraction>().Moveto(moveTile);

        // 2. 플레이어의 상태를 체크 (플레이어가 현재 잡기 CC에 걸려있는지)
        if (Player.Instance.stats.IsPlayerGrabbed())
        {
            Debug.Log("잡기 성공! 연계 패턴을 생성합니다.");
            isGrabSuccess = true;

            // 3. 연계 패턴 턴들을 동적으로 추가
            turnGenerators.Add(MakeGrabTurn);  // 뒤로 돌아서
            turnGenerators.Add(MakeGrabBlow); // 불어서 공격

            // 패턴이 종료되지 않도록 isFinished를 false로 유지 (CompleteTurn에서 체크함)
            isFinished = false;
        }
        else
        {
            Debug.Log("잡기 실패. 패턴을 종료합니다.");
            isFinished = true; // 다음 턴 없이 종료
        }
    }

    HexTile targetTile;
    HexTile backTile;
    public bool isGrabSuccess;
    HashSet<HexTile> rushRange = new();

    public BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        var current = ai.bossController.GetCurrentTile();
        var playerTile = ai.bossController.GetPlayerTile();

        (List<HexTile> result, HexTile tile) = TileRayHelper.GetRayTilesForRush(current, playerTile, 2, true);

        targetTile = tile;
        backTile = current;
        rushRange.AddRange(result);

        return BossPatternBuilder.Create(result).SetDamage(0).Build();
    }

    public BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        Debug.Log(rushRange.Count);
        return BossPatternBuilder.Create(rushRange.ToList()).SetDamage(10).Build();
    }


    public BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        return BossPatternBuilder.Create(rushRange.ToList()).SetDamage(10).SetGrab().Build();
    }

    public BossPatternTurnInfo MakeGrabTurn(BossAI ai)
    {
        ai.bossAnimation.RotateToTile(backTile);
        HexTile playerMovetile = TileDirectionHelper.Instance.GetFrontTile(ai.bossController.GetCurrentTile(), backTile);
        Player.Instance.move.MoveToTile(new PlayerMoveInfo(playerMovetile, false, ignoreDistance: true));

        return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    public BossPatternTurnInfo MakeGrabBlow(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossController.GetCurrentTile(), backTile, 10, 160);

        return BossPatternBuilder.Create(attackRange).SetDamage(10f).SetKnockback(10, true).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }
    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}