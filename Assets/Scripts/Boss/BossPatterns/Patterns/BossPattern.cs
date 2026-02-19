using System;
using System.Collections.Generic;
using System.Linq;

public abstract class BossPattern
{
    protected int currentTurn;
    protected bool isFinished;

    protected bool isTileFixed = false;
    protected HexTile fixedPlayerTile;

    protected List<Func<BossAI, BossPatternTurnInfo>> turnGenerators = new();
    protected BossPatternTurnInfo currentTurnInfo;

    public bool IsFinished => isFinished;
    public int CurrentTurn => currentTurn;
    public BossPatternTurnInfo CurrentTurnInfo => currentTurnInfo;

    // 패턴 시작
    public virtual void OnStartPattern(BossAI ai)
    {
        currentTurn = 0;
        isFinished = false;

        if (isTileFixed)
        {
            fixedPlayerTile = ai.bossController.GetPlayerTile();
        }
    }

    protected virtual void OnBeforeGenerateTurn(BossAI ai) { }

    // 이번 턴 데이터 생성 (예고 단계)
    public virtual BossPatternTurnInfo GenerateTurn(BossAI ai)
    {
        if (isFinished)
            return null;

        if (currentTurn >= turnGenerators.Count)
        {
            isFinished = true;
            return null;
        }

        OnBeforeGenerateTurn(ai);

        currentTurnInfo = turnGenerators[currentTurn](ai);

        currentTurn++;

        return currentTurnInfo;
    }

    // 이번 턴 실행 완료 후 호출
    public virtual void CompleteTurn()
    {
        if (isFinished)
            return;

        currentTurn++;

        if (currentTurn >= turnGenerators.Count)
        {
            isFinished = true;
        }
    }

    public abstract void PerformActionAnimation(BossAnimation animation);
    public virtual void OnPatternEnd(BossAI ai) { }
    public virtual void OnAfterTurnExecuted(BossAI ai) { }

    public virtual void Reset()
    {
        currentTurn = 0;
        isFinished = false;
        currentTurnInfo = null;
    }

    public virtual void ProcessCounter(bool isSuccess) { }

    protected virtual BossPatternTurnInfo MakeIdleTurn(BossAI ai)
    {
        return BossPatternTurnBuilder.Create(new List<HexTile>()).SetDamage(0).Build();
    }
}

public enum BossPatternCheck
{
    Regular,
    Assigned,
    Forced
}

