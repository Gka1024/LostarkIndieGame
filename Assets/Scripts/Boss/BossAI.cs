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

    private void Start()
    {
        bossPhaseController.Initialize();
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

        currentPattern.OnAfterTurnExecuted(this);   // 🔥 이동형 패턴 대응

        currentPattern.CompleteTurn();

        if (currentPattern.IsFinished)
        {
            currentPattern.OnPatternEnd(this);
            currentPattern = null;
        }

        currentTurnInfo = null; // 🔥 중요
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

    private void SetPattern(BossPattern pattern)
    {
        currentPattern = pattern;
        currentPattern?.Reset();
        currentPattern?.OnStartPattern(this);
    }

    private bool IsBossCrowdControlled()
    {
        return bossStatus.IsBossTaunted() || bossStatus.IsBossGroggied();
    }

    public Boss GetBoss()
    {
        return boss;
    }
}


public class BossDamageInfo
{
    public float damage;
    public float stagger;
    public int destroy;

    public BossDamageInfo(float damage, float stagger, int destroy)
    {
        this.damage = damage;
        this.stagger = stagger;
        this.destroy = destroy;
    }
}
