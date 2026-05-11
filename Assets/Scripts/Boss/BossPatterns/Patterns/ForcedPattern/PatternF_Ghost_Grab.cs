using System.Collections.Generic;
using UnityEngine;

public class PatternF_Ghost_Grab : BossPattern
{
    private bool isGrabSuccess = false;
    private HexTile blowTile;

    public PatternF_Ghost_Grab()
    {
        turnGenerators.Add(MakeIdleTurn); // 예고 턴
        turnGenerators.Add(MakeGrabAttempt); // 실제 잡기 시도
    }

    private BossPatternTurnInfo MakeGrabAttempt(BossAI ai)
    {
        // 섹터 범위 계산
        List<HexTile> grabRange = TileDirectionHelper.Instance.GetSectorTiles(
            ai.bossInteraction.GetCurrentTile(),
            Player.Instance.move.GetCurrentTile(), 3, 200);

        // 잡기 판정 데이터 생성
        return BossPatternBuilder.Create(grabRange)
            .SetDamage(10) // 초기 데미지
            .SetGrab()     // 잡기 상태 부여 (PlayerStats에서 처리하도록 설계됨)
            .Build();
    }

    // [중요] 플레이어가 공격에 맞은 후 호출됨
    public override void OnAfterTurnExecuted(BossAI ai)
    {
        // 1. 현재 턴이 잡기 시도 턴(인덱스 1)이었는지 확인
        if (currentTurn == 1)
        {
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
    }

    private BossPatternTurnInfo MakeGrabTurn(BossAI ai)
    {
        HexTile playerTile = Player.Instance.move.GetCurrentTile();
        HexTile turnTile = HexTileManager.Instance.GetTileByCube(playerTile.CubeCoord * -1);

        blowTile = turnTile;

        ai.bossAnimation.RotateToTile(turnTile);
        Player.Instance.move.MoveToTile(new PlayerMoveInfo(turnTile, false, ignoreDistance: true));

        return BossPatternBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakeGrabBlow(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), blowTile, 6, 110);

        return BossPatternBuilder.Create(attackRange).SetDamage(1).SetKnockback(10, true).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

    }
}