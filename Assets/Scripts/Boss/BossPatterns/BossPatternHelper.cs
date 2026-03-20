using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossPatternHelper : MonoBehaviour
{
    private Boss boss;
    private BossAI bossAI;
    private BossStats bossStats;
    private BossStatus bossStatus;
    private BossAnimation bossAnimation;

    public ObjectManager objectManager;

    [Header("Ghost Sphere Create Pattern")]
    [SerializeField] private GameObject ghostSpherePrefab;

    [Header("Pillar Create Pattern")]
    [SerializeField] private GameObject displayName;
    [SerializeField] private List<HexTile> tile_PillarCreate1;


    [Header("Tile Break Pattern")]
    [SerializeField] private List<HexTile> tile_ToBreak_Down;
    [SerializeField] private List<HexTile> tile_ToBreak_Up;
    [SerializeField] private List<HexTile> tile_ToBreak_Middle;
    [SerializeField] private HexTile tileBossLand_Down;
    [SerializeField] private HexTile tileBossLand_Up;

    private bool isDownTileBroke;
    private bool isUpTileBroke;

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
        objectManager.CreatePillarForImposter(tile_PillarCreate1);
    }

    // ================ BreakTiles

    public List<HexTile> GetBreakTilesDown()
    {
        return tile_ToBreak_Down;
    }

    public List<HexTile> GetBreakTilesUp()
    {
        return tile_ToBreak_Up;
    }

    public List<HexTile> GetBreakTilesMiddle()
    {
        return tile_ToBreak_Middle;
    }

    public HexTile GetDownBossTile()
    {
        return tileBossLand_Down;
    }

    public HexTile GetUpBossTile()
    {
        return tileBossLand_Up;
    }

    public void DestroyDownTiles()
    {
        foreach (HexTile tile in tile_ToBreak_Down)
        {
            tile.DestroyTile();
        }
        isDownTileBroke = true;

        if (isUpTileBroke)
        {
            DestroyMiddleTiles();
        }
    }

    public void DestroyUpTiles()
    {
        foreach (HexTile tile in tile_ToBreak_Up)
        {
            tile.DestroyTile();
        }
        isUpTileBroke = true;

        if (isDownTileBroke)
        {
            DestroyMiddleTiles();
        }
    }

    private void DestroyMiddleTiles()
    {
        foreach (HexTile tile in tile_ToBreak_Middle)
        {
            tile.DestroyTile();
        }
    }

    // =========================================================
    // ================== 접근자 (필요 최소한만) =================
    // =========================================================

    public BossAI GetBossAI() => bossAI;
    public BossStatus GetStatus() => bossStatus;
    public BossStats GetStats() => bossStats;
}
