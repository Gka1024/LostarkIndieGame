using UnityEngine;

public class BossPatternHelper : MonoBehaviour
{
    private Boss boss;
    private BossAI bossAI;
    private BossStats bossStats;
    private BossStatus bossStatus;
    private BossAnimation bossAnimation;

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

    public void MakeBossCounter(int duration = 3)
    {
        bossAnimation.FlashCounterBlueLight();
        bossStats.CounterReady(duration);
    }

    public void NotifyCounterResult(bool isSuccess)
    {
        bossAI.NotifyCounterResult(isSuccess);
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

    // =========================================================
    // ================== Ghost Sphere (패턴4용 실행) ===========
    // =========================================================

    public void SpawnGhostSphere(HexTile tile = null)
    {
        if(tile == null)
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

    // =========================================================
    // ================== 접근자 (필요 최소한만) =================
    // =========================================================

    public BossAI GetBossAI() => bossAI;
    public BossStatus GetStatus() => bossStatus;
    public BossStats GetStats() => bossStats;
}
