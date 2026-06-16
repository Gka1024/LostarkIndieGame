using System;
using System.Collections.Generic;
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
    [SerializeField] private List<HexTile> tile_PillarCreateLarge;
    [SerializeField] private List<HexTile> tile_PillarCreateSmall;
    [SerializeField] private List<HexTile> tile_PillarBackLarge;
    [SerializeField] private List<HexTile> tile_PillarBackSmall;

    [Header("Tile Break Pattern")]
    [SerializeField] private List<HexTile> tile_ToBreak_Down;
    [SerializeField] private List<HexTile> tile_ToBreak_Up;
    [SerializeField] private List<HexTile> tile_ToBreak_Middle;
    [SerializeField] private HexTile tileBossLand_Down;
    [SerializeField] private HexTile tileBossLand_Up;

    [Header("Outer Grab Pattern")]
    [SerializeField] private List<HexTile> tile_Outer_Grab;
    [SerializeField] private List<HexTile> tile_Outer_Grab_Alter;
    [SerializeField] private GameObject modeling;

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
    // ================== 방어 / 보호막 접근자 =========================
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

    public void BreakInnerWalls()
    {
        GameManager.Instance.objectManager.DestroyInnerWalls();
    }

    // ================ CreatePillars (PatternF_CreatePillars* 패턴용)

    public int CreatePillars(int index)
    {
        int returnNum = 0;

        switch (index)
        {
            case 0: // 바깥쪽 기둥 만들기
                objectManager.CreatePillarForImposter(tile_PillarCreateLarge);
                break;

            case 1: // 안쪽 기둥 만들기
                objectManager.CreatePillarForImposter(tile_PillarCreateSmall);
                break;

            case 2: // 안쪽 기둥에서 하나 빼고 생성하기
                returnNum = UnityEngine.Random.Range(0, tile_PillarCreateSmall.Count);
                List<HexTile> tiles = new(tile_PillarCreateSmall);
                tiles.Remove(tile_PillarCreateSmall[returnNum]);
                objectManager.CreatePillarForImposter(tiles);

                break;

            default: break;
        }

        return returnNum;
    }

    public List<HexTile> GetPillarTiles(int index)
    {
        switch (index)
        {
            case 0:
                return tile_PillarCreateLarge;

            case 1:
                return tile_PillarCreateSmall;

            default: break;
        }
        return null;
    }

    public List<HexTile> GetPillarSafeTilesLarge()
    {
        return tile_PillarBackLarge;
    }

    public List<HexTile> GetPillarSafeTilesSmall()
    {
        return tile_PillarBackSmall;
    }

    public List<HexTile> GetPillarSafeTilesSmall(int pos)
    {
        List<HexTile> returnTiles = new(tile_PillarBackSmall);

        int targetIndex = (int)pos * 2;

        if (targetIndex + 1 < tile_PillarBackSmall.Count)
        {
            returnTiles.RemoveAt(targetIndex + 1);
            returnTiles.RemoveAt(targetIndex);
        }
        return returnTiles;
    }

    private enum PillarPosition
    {
        RightUP = 0,
        RightDown = 1,
        LeftDown = 2,
        LeftUp = 3,
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
            objectManager.DestroyOuterWallsDown();
        }
        isDownTileBroke = true;

        if (isUpTileBroke)
        {
            DestroyMiddleTiles();
            objectManager.DestroyOuterWallsMiddle();
        }
    }

    public void DestroyUpTiles()
    {
        foreach (HexTile tile in tile_ToBreak_Up)
        {
            tile.DestroyTile();
            objectManager.DestroyOuterWallsUP();
        }
        isUpTileBroke = true;

        if (isDownTileBroke)
        {
            DestroyMiddleTiles();
            objectManager.DestroyOuterWallsMiddle();
        }
    }

    private void DestroyMiddleTiles()
    {
        foreach (HexTile tile in tile_ToBreak_Middle)
        {
            tile.DestroyTile();
        }
    }

    // ================ OuterGrab

    public List<HexTile> GetOuterTiles(int index)
    {
        switch (index)
        {
            case 0:
                return tile_Outer_Grab;

            case 1:
                return tile_Outer_Grab_Alter;

            default: break;
        }

        return null;
    }

    public GameObject GetModelings()
    {
        return modeling;
    }

    // =========================================================
    // ================== 접근자 (필요 최소한만) =================
    // =========================================================

    public BossAI GetBossAI() => bossAI;
    public BossStatus GetStatus() => bossStatus;
    public BossStats GetStats() => bossStats;
}
