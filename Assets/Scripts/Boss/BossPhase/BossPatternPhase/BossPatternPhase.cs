using System.Collections.Generic;
using UnityEngine;

public abstract class BossPatternPhase
{
    protected BossAI ai;

    public abstract int PhaseNumber { get; }

    private int continuousMoveCount = 0;
    protected const int farDistanceThreshold = 5; 

    public void Init(BossAI ai)
    {
        this.ai = ai;
        RegisterRegularPatterns();
        RegisterOpeningPatterns();
        RegisterAssignedPattern();
    }

    protected List<BossPattern> patterns = new();
    protected Queue<BossPattern> openingPatternsQueue = new();
    protected List<AssignedPatternRule> assignedPatterns = new();

    protected abstract void RegisterRegularPatterns(); 
    protected abstract void RegisterOpeningPatterns(); 
    protected abstract void RegisterAssignedPattern(); 

    protected void RegisterRegularPattern(BossPattern pattern)
    {
        patterns.Add(pattern);
    }

    protected void RegisterOpeningPattern(BossPattern pattern)
    {
        openingPatternsQueue.Enqueue(pattern);
    }

    public virtual BossPattern GetNextPattern()
    {
        if (openingPatternsQueue.Count > 0)
        {
            return openingPatternsQueue.Dequeue();
        }

        if (PhaseNumber != 4 && IsPlayerTooFar())
        {
            if (continuousMoveCount < 3)
            {
                continuousMoveCount++;
                Debug.Log($"[패턴 변형] 플레이어가 너무 멀어 이동 패턴 실행 (연속 {continuousMoveCount}회)");
                
                return new Pattern_Move();
            }
            else
            {
                Debug.Log("[패턴 변형] 이동 패턴을 3회 연속 실행하여 조건을 무시하고 일반 패턴을 굴립니다.");
            }
        }

        BossPattern selectedPattern = GetRandomPattern();
        
        if (selectedPattern.GetType().Name != "Pattern_Move")
        {
            continuousMoveCount = 0;
        }
        else
        {
            continuousMoveCount++; // 랜덤으로 뽑았는데 하필 또 Move 패턴이어도 누적 카운트 증가
        }

        return selectedPattern;
    }

    public virtual BossPattern GetRandomPattern()
    {
        if (patterns.Count == 0)
        {
            Debug.LogError("등록된 통상 패턴이 없습니다!");
            return null;
        }

        int rand = UnityEngine.Random.Range(0, patterns.Count);
        Debug.Log($"Pattern : {patterns[rand]}");
        return patterns[rand];
    }

    protected virtual bool IsPlayerTooFar()
    {
        if (Player.Instance == null || ai == null) return false;
        float distance = HexTileManager.Instance.GetTileDistance(Player.Instance.move.GetCurrentTile(), ai.bossController.GetCurrentTile());

        return distance >= farDistanceThreshold;
    }

    public virtual void OnEnter() 
    {
        continuousMoveCount = 0; // 페이즈 진입 시 초기화
    }
    
    public virtual void OnExit() { }
}