using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternList : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private BossStats bossStats;

    public List<BossPattern> BossPatternList1 = new(); // 130줄(바훈투르) 전까지
    public List<BossPattern> BossPatternList2 = new(); // 85줄(지파) 전까지
    public List<BossPattern> BossPatternList3 = new(); // 45줄(2지파) 전까지
    public List<BossPattern> BossPatternList4 = new(); // 30줄(2지파) 이후
    public List<BossPattern> BossPatternList5 = new(); // 발탄 유령

    public List<ForcedPatternCondition> forcedPatterns = new();

    [SerializeField] private BossPattern currentPattern;
    private Queue<BossPattern> queuedPatterns = new(); // 현재 큐에 들어가 있는 패턴들

    private bool phase2Entered = false; // 히히유령


    void Awake()
    {
        RegisterBossPattern();
    }

    private void RegisterBossPattern()
    {
        RegisterAssignedPattern();
        RegisterBossPattern1();
        RegisterBossPattern2();
        RegisterBossPattern3();
        RegisterBossPattern4();
        RegisterBossPattern5();
    }

    private void RegisterAssignedPattern()
    {
        EnqueueEncounterPatterns();


    }

    private void EnqueueEncounterPatterns()
    {
        queuedPatterns.Enqueue(new AssignedPatternNo1());
        queuedPatterns.Enqueue(new AssignedPatternNo2());
        queuedPatterns.Enqueue(new AssignedPatternNo3());
        queuedPatterns.Enqueue(new AssignedPatternNo4());
        queuedPatterns.Enqueue(new AssignedPatternNo5());
        queuedPatterns.Enqueue(new AssignedPatternNo6());
    }

    private void RegisterBossPattern1()
    {
        BossPatternList1.Add(new PatternNo1());
    }

    private void RegisterBossPattern2()
    {

    }

    private void RegisterBossPattern3()
    {

    }

    private void RegisterBossPattern4()
    {

    }

    private void RegisterBossPattern5()
    {

    }

    public BossPattern GetNextPattern()
    {
        SelectNextPattern();
        return currentPattern;
    }

    public void SelectNextPattern()
    {
        // 1️⃣ 큐에 예약된 패턴이 있다면 먼저 실행
        if (queuedPatterns.Count > 0)
        {
            currentPattern = queuedPatterns.Dequeue();
            Debug.Log($"[BossPatternList] 큐에서 패턴 실행: {currentPattern.GetType().Name}");
            return;
        }

        // 2️⃣ 체력 조건에 따라 강제 패턴(여러 개 가능) 등록
        foreach (var forced in forcedPatterns)
        {
            if (!forced.hasUsed && bossStats.GetBossHPByLine() <= forced.triggerHpLine)
            {
                if (forced.patternsToUse != null && forced.patternsToUse.Count > 0)
                {
                    foreach (var pattern in forced.patternsToUse)
                    {
                        queuedPatterns.Enqueue(pattern);
                        Debug.Log($"[BossPatternList] 강제 패턴 예약: {pattern.GetType().Name}");
                    }

                    currentPattern = queuedPatterns.Dequeue();
                    forced.hasUsed = true;

                    if (!phase2Entered && forced.triggerHpLine <= 15)
                    {
                        phase2Entered = true;
                        bossAI.EnterPhase2();
                        Debug.Log("[BossPatternList] 2페이즈 진입!");
                    }
                    return;
                }
            }
        }

        // 3️⃣ 일반 체력 구간별 패턴 선택
        List<BossPattern> availableBossPatternsList = GetAvailablePatternsByHP();

        if (availableBossPatternsList == null || availableBossPatternsList.Count == 0)
        {
            Debug.LogWarning("[BossPatternList] 사용 가능한 패턴이 없습니다.");
            currentPattern = null;
            return;
        }

        currentPattern = availableBossPatternsList[Random.Range(0, availableBossPatternsList.Count)];
        Debug.Log($"[BossPatternList] 일반 패턴 선택: {currentPattern.GetType().Name}");
    }


    private List<BossPattern> GetAvailablePatternsByHP()
    {
        float curHP = bossStats.GetBossHPByLine();

        if (curHP >= 130) return BossPatternList1;
        if (curHP >= 85) return BossPatternList2;
        if (curHP >= 30) return BossPatternList3;
        if (curHP >= 15) return BossPatternList4;

        return BossPatternList5;
    }

    public BossPattern GetCurrentPattern()
    {
        return currentPattern;
    }
}


public class ForcedPatternCondition
{
    public float triggerHpLine;

    public List<BossPattern> patternsToUse;

    public bool hasUsed = false;
}