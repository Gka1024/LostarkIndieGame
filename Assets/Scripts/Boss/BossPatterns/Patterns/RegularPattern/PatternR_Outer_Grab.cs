using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PatternR_Outer_Grab : BossPattern
{
    private bool isGrabSuccess;
    // 생성된 오브젝트들을 관리할 리스트 (배열보다 리스트가 안전합니다)
    private List<GameObject> outerModelings = new List<GameObject>();

    public PatternR_Outer_Grab()
    {
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
    }

    public override void OnStartPattern(BossAI ai)
    {
        // 새로운 패턴 시작 시 이전 리스트 정리
        ClearModelings();
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 4)
        {
            if (Player.Instance.stats.IsPlayerGrabbed())
            {
                Debug.Log("잡기 성공! 연계 패턴을 생성합니다.");
                isGrabSuccess = true;
                turnGenerators.Add(MakeGrabBlow);
                isFinished = false;
            }
            else
            {
                Debug.Log("잡기 실패. 패턴을 종료합니다.");
                ClearModelings(); // 실패 시 소환수 제거
                isFinished = true;
            }
        }
    }

    HashSet<HexTile> totalAttackRange = new();
    int ranNum = 0;

    public BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        totalAttackRange.Clear();
        ranNum = UnityEngine.Random.Range(0, 2);
        List<HexTile> spawnTiles = ai.bossPatternHelper.GetOuterTiles(ranNum);
        HexTile centerTile = HexTileManager.Instance.GetTileByCube(Vector3Int.zero);

        foreach (HexTile sTile in spawnTiles)
        {
            // 각 분신별 공격 범위 계산 (반지름 2)
            List<HexTile> individualRange = HexTileManager.Instance.GetTilesWithinRange(sTile, 2);

            // 전체 범위에 합치기
            foreach (var t in individualRange)
                if (!totalAttackRange.Contains(t)) totalAttackRange.Add(t);

            // 분신 소환 및 데이터 전달
            SpawnSingleMonster(ai, sTile, individualRange, centerTile);
        }

        return BossPatternBuilder.Create(totalAttackRange.ToList()).SetDamage(0).Build();
    }

    public BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        return BossPatternBuilder.Create(totalAttackRange.ToList()).SetDamage(0).Build();
    }

    public BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        return BossPatternBuilder.Create(totalAttackRange.ToList()).SetDamage(0).SetGrab().Build();
    }

    public BossPatternTurnInfo MakeGrabBlow(BossAI ai)
    {
        List<HexTile> attackRange = HexTileManager.Instance.GetAllTiles();
        return BossPatternBuilder.Create(attackRange).SetDamage(1f).SetKnockback(10, true).Build();
    }

    private void SpawnSingleMonster(BossAI ai, HexTile spawnTile, List<HexTile> range, HexTile center)
    {
        GameObject model = ai.bossPatternHelper.GetModelings();
        GameObject obj = Object.Instantiate(model, spawnTile.transform.position, Quaternion.identity);

        var script = obj.GetOrAddComponent<OuterGrabMonster>();
        var scriptAnim = obj.GetOrAddComponent<BossAnimation>();
        script.Init(this, spawnTile, range, ai);
        scriptAnim.SetGhostAppearance(true, 0.9f);

        obj.transform.LookAt(center.transform.position);
        outerModelings.Add(obj);
    }

    // [핵심] 카운터 시 호출되어 공격 범위를 깎아냄
    public void RemoveAttackRange(List<HexTile> tilesToRemove)
    {
        // 전체 범위에서 해당 분신의 범위를 제외
        totalAttackRange = new HashSet<HexTile>(totalAttackRange.Except(tilesToRemove));
    }

    public override void OnPatternEnd(BossAI ai)
    {
        ClearModelings(); // 패턴 완전히 종료 시 정리
    }

    private void ClearModelings()
    {
        foreach (var obj in outerModelings)
        {
            if (obj != null) UnityEngine.Object.Destroy(obj);
        }
        outerModelings.Clear();
    }

    public void CancelPattern(BossAI ai, List<HexTile> attackRange)
    {
        Debug.Log("패턴 취소됨: 모든 외곽 모델링을 제거하고 턴을 조기 종료합니다.");
        ClearModelings();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        return;
    }
}