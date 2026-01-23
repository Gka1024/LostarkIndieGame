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
    public BossPatternList bossPatternList;
    public BossPatternHelper bossPatternHelper;

    public BossPattern currentPattern;
    public BossPattern nextPattern;
    public bool isPatternNull;

    public GameObject GhostSpherePrefab;
    GameObject sphereCurrent;

    private void Start()
    {

    }

    public void OnTurnStart()
    {
        PerformAction();
    }

    public void OnTurnEnd()
    {
        ExecuteAction();
    }

    // ==== 보스 패턴 관련 로직

    public void FindNextAction()
    {
        if (GetBossTaunted() || GetBossGroggied())
        {
            return;
        }

        if (nextPattern != null)
        {
            currentPattern = nextPattern;
            nextPattern = null;
            return;
        }

        if (currentPattern == null)
        {
            SetPattern(GetNextPattern()); // 보스의 패턴 찾기
        }
    }

    public void PerformAction()
    { // 턴이 시작할 때 실행되는 함수. 보스의 피격 범위를 알려주고, 패턴이 끝나면 새로운 패턴을 호출하는 책임을 지님
        if (GetBossTaunted() || GetBossGroggied())
        {
            return;
        }

        if (currentPattern != null)
        {
            if (currentPattern.isFinished)
            {
                currentPattern.OnPatternEnd(this);
                ResetCurrentPattern();
            }
        }

        if (currentPattern == null) FindNextAction();

        currentPattern.OnPatternTurn(this);
    }

    public void ExecuteAction()
    { // 턴이 끝날 때 실행되는 함수. 실제 플레이어에게 데미지를 주거나, 보스가 행동하는 애니메이션을 재생함. 
        if (!IsBossCanAction())
        {
            currentPattern?.ExecutePattern(this);
            currentPattern?.PerformActionAnimation(bossAnimation);
        }
    }

    public void SetPattern(BossPattern pattern)
    {
        currentPattern = pattern;
        currentPattern.Reset();
        currentPattern.OnStartPattern(this);
    }

    private BossPattern GetNextPattern()
    {
        if (bossPatternList == null) Debug.LogError("BossPatternList가 연결되지 않았습니다.");

        BossPattern nextPattern = bossPatternList.GetNextPattern();

        if (nextPattern == null) Debug.LogError("BossPatternList에서 패턴을 가져오지 못했습니다. ");

        return nextPattern;
    }

    public void GetCurrentPatternDebug()
    {
        Debug.Log(currentPattern);
    }

    public void ResetCurrentPattern()
    {
        currentPattern = null;
    }

    private bool IsBossCanAction()
    {
        return GetBossTaunted() || GetBossGroggied();
    }

    // 보스 디버프

    private bool GetBossTaunted()
    {
        return bossStatus.IsBossTaunted();
    }

    private bool GetBossGroggied()
    {
        return bossStatus.IsBossGroggied();
    }

    public void SetBossTaunted()
    {
        ResetCurrentPattern();
    }

    public void RecoverFromTaunted()
    {

    }

    public void RecoverFromGroggy()
    {
        Debug.Log("Recover From Groggy");
    }

    // ==== 특수 패턴 관련 로직

    public void EnterPhase2()
    {
        bossStats.EnterPhase2();
    }

    // ==== 공통 로직

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
