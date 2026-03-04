using System.Collections.Generic;
using UnityEngine;

public class BossPatternHelper : MonoBehaviour
{
    private Boss boss;
    private BossAI bossAI;
    private BossStats bossStats;
    private BossStatus bossStatus;
    private BossAnimation bossAnimation;

    public ObjectManager objectManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject ghostSpherePrefab;

    private GameObject currentSphere;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossAI = GetComponent<BossAI>();
        bossStats = GetComponent<BossStats>();
        bossStatus = GetComponent<BossStatus>();
        bossAnimation = GetComponent<BossAnimation>();
    }

    // =========================================================
    // ================== 카운터 관련 ==========================
    // =========================================================

    public void MakeBossCounter(int duration)
    {
        bossAnimation.FlashCounterBlueLight();
    }

    // =========================================================
    // ================== 방어 / 보호막 =========================
    // =========================================================

    public void SetBossDefence(float ratio)
    {
        bossStats.SetDefenceRatio(ratio);
    }

    public void ResetBossDefence()
    {
        bossStats.SetDefenceRatio(1f);
    }

    public void CreateBossShield(float shield)
    {
        bossStats.CreateShield(shield);
    }

    public void RemoveBossShield()
    {
        bossStats.RemoveShield();
    }

    public bool HasBossShield()
    {
        return bossStats.HasShield();
    }

    public void NotifyShieldBroken()
    {
        bossAI.NotifyShieldBroken();
    }

    // ================== Ghost Sphere (패턴4용 실행) ===========

    public void SpawnGhostSphere(HexTile tile = null)
    {
        if (tile == null)
        {
            tile = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());
        }

        var obj = Instantiate(ghostSpherePrefab);

        var sphere = obj.GetComponent<GhostSphereScript>();
        sphere.Initialize(tile, bossAI);

        sphere.OnSphereBroken += bossAI.NotifySummonedObjectDestroyed;
    }

    public HexTile GetCurrentSphereTile()
    {
        if (currentSphere == null) return null;

        return currentSphere
            .GetComponent<GhostSphereScript>()
            .GetHexTile();
    }

    public void ClearCurrentSphere()
    {
        currentSphere = null;
    }

    // ========= BreakWalls & BreakPillars (PatternF_Break_Walls_Pillars 용 실행)=========

    public void BreakAllWalls()
    {
        GameManager.Instance.objectManager.BreakAllWalls();
    }

    public void BreakAllPillars()
    {
        GameManager.Instance.objectManager.BreakAllPillars();
    }

    // ================ CreatePillars (PatternF_CreatePillars* 패턴용)

    public void CreatePillars()
    {
        objectManager.CreatePillarForImposter();
    }

    // =========================================================
    // ================== 접근자 (필요 최소한만) =================
    // =========================================================

    public BossAI GetBossAI() => bossAI;
    public BossStatus GetStatus() => bossStatus;
    public BossStats GetStats() => bossStats;
}
