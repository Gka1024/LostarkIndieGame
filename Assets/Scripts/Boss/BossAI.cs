using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Boss boss;
    public BossController bossController;

    public BossStats bossStats;
    public BossStatus bossStatus;
    public BossInteraction bossInteraction;
    public BossAnimation bossAnimation;

    public BossPhaseController bossPhaseController;

    public BossPattern currentPattern;
    public BossPatternTurnInfo currentTurnInfo;
    public BossPatternHelper bossPatternHelper;

    private bool isTaunted;
    private bool isGroggied;

    private void Start()
    {
        bossPhaseController.Initialize();

        // 🔥 이벤트 기반 연결 (추천 구조)
        bossStatus.OnTauntApplied += SetBossTaunted;
        bossStatus.OnTauntRecovered += RecoverFromTaunt;

        bossStatus.OnGroggyApplied += SetBossGroggy;
        bossStatus.OnGroggyRecovered += RecoverFromGroggy;
    }

    // ===============================
    // 턴 시작 (예고 단계)
    // ===============================

    public void OnTurnStart()
    {
        if (IsBossCrowdControlled())
            return;

        if (currentPattern == null)
            SetPattern(bossPhaseController.GetNextPattern());

        if (currentPattern == null)
            return;

        currentTurnInfo = currentPattern.GenerateTurn(this);

        bossController.ShowAttackPreview(currentTurnInfo);
    }

    // ===============================
    // 턴 종료 (실행 단계)
    // ===============================

    public void OnTurnEnd()
    {
        if (IsBossCrowdControlled())
            return;

        if (currentTurnInfo == null || currentPattern == null)
            return;

        ApplyTurn(currentTurnInfo);

        currentPattern.PerformActionAnimation(bossAnimation);

        currentPattern.OnAfterTurnExecuted(this);

        currentPattern.CompleteTurn();

        if (currentPattern.IsFinished)
        {
            Debug.Log("패턴이 끝났습니다.");
            currentPattern.OnPatternEnd(this);
            currentPattern = null;
        }

        currentTurnInfo = null;
    }

    // ===============================
    // 실제 데미지 처리
    // ===============================

    private void ApplyTurn(BossPatternTurnInfo info)
    {
        Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        HexTile playerTile = player.move.GetCurrentTile();

        bossController.ClearAttackPreview(info);

        if (info.IsSpecial)
        {
            if (!player.stats.playerBuffState.HasPlayerSpecialBuffs("아크투르스의 가호"))
            {
                player.stats.KillPlayerInstantly();
                return;
            }
        }

        if (!info.TargetTiles.Contains(playerTile))
            return;

        ApplyPlayerDamage(player, info);

        if (info.IsKnockback)
        {
            // 넉백 처리
        }

        if (info.IsDownAttack)
        {
            // 다운 처리
        }
    }

    private void ApplyPlayerDamage(Player player, BossPatternTurnInfo info)
    {
        player.stats.GetPlayerDamage(info.ToPlayerDamageInfo());
    }

    // ===============================
    // CC 처리
    // ===============================

    private bool IsBossCrowdControlled()
    {
        return isTaunted || isGroggied;
    }

    public void SetBossTaunted(GameObject target)
    {
        isTaunted = true;

        // 현재 패턴 예고 제거 (꼬임 방지)
        if (currentTurnInfo != null)
            bossController.ClearAttackPreview(currentTurnInfo);

        currentTurnInfo = null;

        // 필요하다면 타겟 강제 지정 로직 추가
        // 예: bossInteraction.SetForcedTarget(bossStatus.TauntedTarget);
    }

    public void RecoverFromTaunt()
    {
        isTaunted = false;

        // 타겟 정상화
        // bossInteraction.ResetTarget();

        // 다음 턴부터 정상 패턴 진행
    }

    public void SetBossGroggy()
    {
        isGroggied = true;

        // 그로기 들어가면 패턴 중단
        if (currentTurnInfo != null)
            bossController.ClearAttackPreview(currentTurnInfo);

        currentPattern = null;
        currentTurnInfo = null;

        //bossAnimation.PlayGroggyAnimation();
    }

    public void RecoverFromGroggy()
    {
        isGroggied = false;

        // bossAnimation.PlayRecoverAnimation();

        // 그로기 끝난 뒤 새 패턴 시작
        currentPattern = null;
        currentTurnInfo = null;
    }

    public void InterruptCurrentPattern()
    {
        if (currentPattern == null)
            return;

        Debug.Log($"Pattern Interrupted: {currentPattern.GetType().Name}");

        currentPattern.OnInterrupted(this);

        currentPattern = null;

    }

    // ===============================

    private void SetPattern(BossPattern pattern)
    {
        currentPattern = pattern;
        currentPattern?.Reset();
        currentPattern?.OnStartPattern(this);
    }

    public void NotifySummonedObjectDestroyed(GhostSphereScript sphere)
    {
        currentPattern?.OnSummonedObjectDestroyed(sphere.gameObject);
    }

    public void NotifyCounterResult(bool isSuccess)
    {

    }

    public void NotifyShieldBroken()
    {

    }

    public void NotifyBossDead()
    {

    }

    public void NotifyDestroyResult(bool isSuccess)
    {
        if (isSuccess)
        {
            InterruptCurrentPattern();

            bossStatus.MakeBossGroggy(5);
        }
    }

    public Boss GetBoss()
    {
        return boss;
    }
}
