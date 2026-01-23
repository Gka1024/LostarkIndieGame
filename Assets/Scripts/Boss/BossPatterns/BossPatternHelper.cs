using UnityEngine;

public class BossPatternHelper : MonoBehaviour
{
    public Boss boss;
    public BossAI bossAI;
    public BossStats bossStats;
    public BossAnimation bossAnimation;

    public GameObject GhostSpherePrefab;

    public GameObject sphereCurrent;

    void Start()
    {
        boss = GetComponent<Boss>();
        bossAI = GetComponent<BossAI>();
        bossStats = GetComponent<BossStats>();
        bossAnimation = GetComponent<BossAnimation>();
    }

    public void MakeBossCounter(int duration = 3)
    {
        bossAnimation.FlashCounterBlueLight();
        bossStats.CounterReady(duration);
    }

    public void SuccessCounter(bool isSuccess)
    {
        bossAI.currentPattern.ProcessCounter(isSuccess);
    }

    public void OnSummonedObjectDestroyed(GameObject obj)
    {
        if (bossAI.currentPattern is PatternNo4 curPattern)
        {
            curPattern.BrokenSphere(this);
        }
    }

    public bool CheckBossShield()
    {
        return bossStats.CheckBossShield();
    }

    public void BossShieldBroke()
    {
        if (bossAI.currentPattern is PatternNo4 curPattern)
        {
            curPattern.ShieldBroke();
        }
    }

    // ==== 보스 속성 변환 코드

    public void SetBossDefence(float ratio)
    {
        bossStats.SetDefenceRatio(ratio);
    }

    public void ResetBossDefence()
    {
        bossStats.SetDefenceRatio(1.0f);
    }

    public void MakeBossShield(float shield)
    {
        bossStats.CreateShield(shield);
    }

    public void RemoveBossShield()
    {
        bossStats.RemoveShield();
    }

    // ==== 패턴 4번용 코드

    public void SpawnGhostSphere(HexTile tile = null)
    {
        sphereCurrent = Instantiate(GhostSpherePrefab, tile.transform.position, Quaternion.identity);
        sphereCurrent.GetComponent<GhostSphereScript>().SetPosition(tile);
    }

    public HexTile GetSphereHexTile()
    {
        if (sphereCurrent != null)
        {
            return sphereCurrent.GetComponent<GhostSphereScript>().GetHexTile();
        }
        return null;
    }

    public void GhostSphereBroke()
    {
        if (bossAI.currentPattern is PatternNo4 curPattern)
        {
            curPattern.BrokenSphere(this);
        }
    }

}
