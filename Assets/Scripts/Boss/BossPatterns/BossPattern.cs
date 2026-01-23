using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BossPattern
{
    private BossAnimation bossAnimation;

    protected int totalTurns;
    protected int currentTurn;
    public bool isFinished;

    protected bool isCounterSuccess;

    protected bool isTileFixed = false;
    protected HexTile fixedPlayerTile;

    protected List<Func<BossAI, BossPatternTurnInfo>> turnGenerators = new();
    protected BossPatternTurnInfo turnInfo;

    public virtual void OnStartPattern(BossAI ai)
    {
        currentTurn = 0;

        if (isTileFixed)
        {
            fixedPlayerTile = ai.bossController.GetPlayerTile(); // 패턴 시작 시 위치 고정
        }
    }

    public virtual void OnPatternTurn(BossAI ai)
    {
        Debug.Log($"currentTurn : {currentTurn} | Pattern : {ai.currentPattern}");

        if (currentTurn >= turnGenerators.Count)
        {
            isFinished = true;

            return;
        }

        turnInfo = turnGenerators[currentTurn](ai);

        HexTileManager.Instance.ResetTileColor();

        foreach (HexTile tile in turnInfo.TargetTiles)
        {
            tile.isBossAttackRange = true;
            tile.ResetColor();
        }

        currentTurn++;
    }

    public virtual void ExecutePattern(BossAI ai)
    {
        if(isFinished)
        {
            return;
        }

        Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        HexTile playerTile = player.move.GetCurrentTile();

        foreach (HexTile tile in turnInfo.TargetTiles)
        {
            tile.isBossAttackRange = false;
            tile.ResetColor();
        }

        if (turnInfo.BreakWalls)
        {
            foreach (HexTile tile in turnInfo.TargetTiles)
            {
                tile.BreakWalls(true);
            }
        }

        if (!turnInfo.TargetTiles.Contains(playerTile))
        {
            return;
        }

        PlayerGetDamageInfo info = new PlayerGetDamageInfo(turnInfo.Damage, false, turnInfo.IsKnockback, turnInfo.KnockbackDistance);

        player.stats.GetPlayerDamage(info);
        Debug.Log($"턴 {currentTurn} - 플레이어가 {turnInfo.Damage} 피해를 입습니다!");

        if (turnInfo.IsKnockback)
        {
             Debug.Log($"플레이어가 {turnInfo.IsKnockback}칸 넉백됩니다.");
            ai.bossController.PlayerKnockBack(turnInfo.KnockbackDistance);
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


    public abstract void PerformActionAnimation(BossAnimation animation);
    public abstract void OnPatternEnd(BossAI ai);

    public virtual void Reset()
    {
        currentTurn = 0;
        isFinished = false;
    }

    protected BossPatternTurnInfo CreatePattern(
      BossAI ai,
      (int direction, int count, bool clockwise)[] patterns,
      int damage,
      bool isDown = false,
      bool isGrab = false,
      bool isKnockback = false,
      bool breakWalls = false,
      bool isSpecial = false,
      int knockbackDistance = 0)
    {
        HashSet<HexTile> attackRangeSet = new();
        var current = ai.bossController.GetCurrentTile();
        var facing = isTileFixed ? fixedPlayerTile : ai.bossController.GetPlayerTile();

        foreach (var (dir, count, clockwise) in patterns)
        {
            attackRangeSet.UnionWith(
                HexTileManager.Instance.tileDirectionHelper.GetDistanceTiles(current, facing, dir, count, clockwise)
            );
        }

        return new BossPatternTurnInfo(
            attackRangeSet.ToList(),
            damage,
            isDown,
            isGrab,
            isKnockback,
            breakWalls,
            isSpecial,
            knockbackDistance
        );
    }


    public virtual void ProcessCounter(bool isSuccess) { }

}


public class BossPatternTurnInfo
{
    public List<HexTile> TargetTiles { get; private set; }
    public float Damage { get; private set; }

    public bool IsKnockback { get; private set; }
    public int KnockbackDistance { get; private set; }

    public bool IsGrab { get; private set; }
    public bool IsDown { get; private set; }
    public bool BreakWalls { get; private set; }

    public bool IsSpecial;

    public BossPatternTurnInfo(
        List<HexTile> targetTiles,
        float damage,
        bool isDown = false,
        bool isGrab = false,
        bool isKnockback = false,
        bool breakWalls = false,
        bool isSpecial = false,
        int knockbackDistance = 0)
    {
        TargetTiles = targetTiles;
        Damage = damage;
        IsDown = isDown;
        IsGrab = isGrab;
        BreakWalls = breakWalls;
        IsKnockback = isKnockback;
        KnockbackDistance = knockbackDistance;
        IsSpecial = isSpecial;
    }
}

