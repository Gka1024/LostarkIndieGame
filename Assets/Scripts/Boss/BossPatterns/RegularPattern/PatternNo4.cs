using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternNo4 : BossPattern
{ // 구체 생성 (원혼불) 패턴입니다. 구체를 파괴하면 보스에게 카운터 공격, 20턴 내 쉴드 제거 실패 시 주변에 50 데미지
    public bool isShieldBroken = false;

    public PatternNo4()
    {
        totalTurns = 15;

        for (int i = 0; i < totalTurns; i++)
        {
            turnGenerators.Add(MakePattern0);
        }
        // 21번째 턴에 쉴드가 남아있으면 주변 공격
        turnGenerators.Add(MakePatternFailAttack);
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);

        var current = ai.bossController.GetCurrentTile();

        GameManager.Instance.GetPlayer().GetComponent<PlayerMove>().GetCurrentTile();
        ai.bossPatternHelper.SpawnGhostSphere();
        var facing = ai.bossPatternHelper.GetSphereHexTile();

        ai.bossPatternHelper.SetBossDefence(0.1f);
        ai.bossPatternHelper.MakeBossShield(3000f);

        isShieldBroken = false;
    }

    public override void ExecutePattern(BossAI ai)
    {
        base.ExecutePattern(ai);

        // 20턴 이내에 쉴드가 깨졌으면 마지막 공격 패턴을 건너뜀
        if (currentTurn == totalTurns + 1 && isShieldBroken)
        {
            isFinished = true;
        }
    }

    public void BrokenSphere(BossPatternHelper bossPatternHelper)
    {
        bossPatternHelper.ResetBossDefence();
        bossPatternHelper.RemoveBossShield();
        bossPatternHelper.MakeBossShield(150f);
        isShieldBroken = false;
    }

    private BossPatternTurnInfo MakePattern0(BossAI ai)
    {
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    public void ShieldBroke()
    {
        isShieldBroken = true;
        // 남은 턴 수를 3으로 조정 (현재 턴을 totalTurns - 2로 설정)
        currentTurn = totalTurns - 2;
        for (int i = currentTurn + 1; i < turnGenerators.Count; i++)
        {
            turnGenerators[i] = ai => new BossPatternTurnInfo(new List<HexTile>(), 0);
        }
    }

    // 20턴 내 쉴드 제거 실패 시 주변에 50 데미지
    private BossPatternTurnInfo MakePatternFailAttack(BossAI ai)
    {
        // 쉴드가 남아있으면 주변 타일에 50 데미지
        if (!isShieldBroken)
        {
            var bossTile = ai.bossController.GetCurrentTile();
            var aroundTiles = HexTileManager.Instance.GetTilesWithinRange(bossTile, 1);
            return new BossPatternTurnInfo(aroundTiles, 50);
        }
        // 쉴드가 이미 깨졌으면 아무 일도 없음
        return new BossPatternTurnInfo(new List<HexTile>(), 0);
    }

    public override void OnPatternEnd(BossAI ai)
    {

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

    }
}
