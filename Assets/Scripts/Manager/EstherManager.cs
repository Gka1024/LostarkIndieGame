using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EstherManager : MonoBehaviour
{
    public static EstherManager Instance { get; private set; }

    public GameManager manager;
    public HexTileManager tileManager;

    public Boss boss;
    public Player player;

    [SerializeField] private bool isEstherUsing;

    // 에스더 스킬 오브젝트들
    public GameObject ThirainSkillObject;
    public GameObject WayeSkillObject;
    public GameObject BahunturrSkillObject;
    public GameObject NinaveSkillObject;
    public GameObject InannaSkillObject;
    public GameObject AzenaSkillObject;
    public GameObject ShandiSkillObject;
    public GameObject KadanAttackSkillObject;
    public GameObject KadanShieldSkillObject;

    // 에스더 카드들
    public GameObject estherCard1;
    public GameObject estherCard2;
    public GameObject estherCard3;

    private GameObject skillObj;
    [SerializeField] private EstherSkill pendingEstherSkill;

    // 에스더 게이지 관련
    public GameObject estherCancelButton;
    public RectTransform estherGaugeMask;
    private float maskFullWidth;
    public float estherGainPerTurn = 14f;

    private const float MAX_ESTHER_VALUE = 350;
    [SerializeField] private float estherValue;

    // 에스더 스킬 타일 선택 관련
    public bool isEstherTileSelected { get; private set; }
    private List<HexTile> selectedEstherTiles = new();

    // 에스더 캐릭터 관련
    public GameObject EstherThirainModel;
    public GameObject EstherBahunturrModel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        maskFullWidth = estherGaugeMask.sizeDelta.x;
        estherValue = MAX_ESTHER_VALUE;
        UpdateEstherBar();
    }

    public void SetEstherValue(float value)
    {
        estherValue = Mathf.Clamp(value, 0, MAX_ESTHER_VALUE);
        UpdateEstherBar();
    }

    public void AddEstherValue(float value)
    {
        estherValue += value;
        estherValue = Mathf.Min(estherValue, MAX_ESTHER_VALUE);
        UpdateEstherBar();
    }

    public void ClearEstherSkill()
    {
        Debug.Log("EstherClear");
        pendingEstherSkill = null;
    }

    private void EstherCardBackgroundShow(bool show)
    {
        estherCard1?.GetComponent<EstherCard>()?.ShowBackground(show);
        // TODO: 추가 카드도 필요 시 활성화
        // estherCard2?.GetComponent<EstherCard>()?.ShowBackground(show);
        // estherCard3?.GetComponent<EstherCard>()?.ShowBackground(show);
    }

    public void UpdateEstherBar()
    {
        float estherRatio = Mathf.Clamp01(estherValue / MAX_ESTHER_VALUE);
        estherGaugeMask.sizeDelta = new Vector2(estherRatio * maskFullWidth, estherGaugeMask.sizeDelta.y);
        EstherCardBackgroundShow(IsEstherFull());
    }

    public bool IsEstherFull()
    {
        return estherValue == MAX_ESTHER_VALUE;
    }

    public void UseEstherSkill(EstherType esther)
    {
        if (!IsEstherFull()) return;
        if (isEstherUsing) return;

        isEstherUsing = true;

        estherCancelButton.SetActive(true);

        switch (esther)
        {
            case EstherType.Sillian:
                StartCoroutine(UseThirainSkill());
                break;

            case EstherType.Waye:
                UseWayeSkill();
                break;

            case EstherType.Bahunturr:
                StartCoroutine(UseBahunturSkill());
                break;

            case EstherType.Ninave:
                // TODO: Add Ninave skill
                break;

            case EstherType.Inanna:
                // TODO: Add Inanna skill
                break;

            case EstherType.Azena:
                // TODO: Add Azena skill
                break;

            case EstherType.Shandi:
                // TODO: Add Shandi skill
                break;

            case EstherType.KadanAttack:
                // TODO: Add Kadan Attack skill
                break;

            case EstherType.KadanShield:
                // TODO: Add Kadan Shield skill
                break;

            default:
                Debug.LogWarning("Invalid Esther skill requested.");
                break;
        }
    }

    public void CancelEsther()
    {
        skillObj = null;
        isEstherUsing = false;
        isEstherTileSelected = false;
        selectedEstherTiles.Clear();
        GameManager.Instance.hexTileSelectHandler.CancelSelection();
        estherCancelButton.SetActive(false);
    }

    public void SetEstherSkill(EstherSkill skill = null)
    {
        pendingEstherSkill = skill;
        isEstherTileSelected = true;
    }

    public void OnTurnEnd()
    {
        if (!IsEstherFull()) AddEstherValue(estherGainPerTurn);

        if (pendingEstherSkill == null) return;

        pendingEstherSkill.OnTurnPassed();
    }

    // 에스더 스킬 데미지, 무력화, 파괴 처리
    public void ProcessEstherSkillDamageData(BossDamageData data)
    {
        if (tileManager.IsBossTile(selectedEstherTiles))
        {
            boss.bossController.GetBossDamageData(data);
        }
    }

    // 에스더 특수 버프 처리

    public void GivePlayerBuff(string buffname, int duration)
    {
        foreach (HexTile tile in selectedEstherTiles)
        {
            if (manager.GetPlayer().GetComponent<PlayerMove>().GetCurrentTile() == tile)
            {
                manager.GetPlayer().GetComponent<PlayerStats>().playerBuffState.AddPlayerSpecialBuff(buffname, duration);
                return;
            }
        }
    }

    // ==== 이하는 구체적인 에스더 스킬의 사용입니다.

    private void ResetEsther()
    {
        estherValue = 0f;
        UpdateEstherBar();
        isEstherUsing = false;
        estherCancelButton.SetActive(false);
    }

    private GameObject InstantiateEsther(GameObject esther, HexTile spawnTile, HexTile targetTile = null)
    {
        Vector3 spawnPos = spawnTile.GetThisSpawnPos() + new Vector3(0, 20f, 0);
        GameObject estherModel = Instantiate(esther, spawnPos, quaternion.identity);

        if (targetTile != null)
        {
            estherModel.GetComponent<EstherAnimationController>().RotateToTile(targetTile);
        }

        return estherModel;
    }

    private IEnumerator UseThirainSkill()
    {
        if (ThirainSkillObject == null)
        {
            Debug.LogError("ThirainSkillObject is not assigned!");
            yield break;
        }

        // ==== 스킬 오브젝트 생성
        skillObj = Instantiate(ThirainSkillObject);
        EstherSkill estherThirain = skillObj.GetComponent<EstherSkill>();
        estherThirain.estherManager = this;

        // ==== 스킬 방향 결정
        estherThirain.SelectTile();
        yield return new WaitUntil(() => manager.hexTileSelectHandler.selectedTiles.Count > 0);

        // ==== 모델링 생성
        HexTile targetTile = manager.hexTileSelectHandler.selectedTile;
        HexTile spawnTile = HexTileManager.Instance.GetNearestTile(manager.GetPlayer().GetComponent<PlayerMove>().GetCurrentTile(), targetTile);
        EstherAnimationController skillController = InstantiateEsther(EstherThirainModel, spawnTile, targetTile).GetComponent<EstherAnimationController>();
        estherThirain.estherAnimationController = skillController;
        estherThirain.SpawnToGround(spawnTile);

        selectedEstherTiles = manager.hexTileSelectHandler.selectedTiles;
        SetEstherSkill(estherThirain);

        estherThirain.Execute();

        ResetEsther();
    }

    private IEnumerator UseBahunturSkill()
    {
        if (BahunturrSkillObject == null)
        {
            Debug.LogError("BahunturrSkillObject is not assigned!");
            yield break;
        }

        skillObj = Instantiate(BahunturrSkillObject);
        EstherSkill estherBahunturr = skillObj.GetComponent<EstherSkill>();
        estherBahunturr.estherManager = this;

        estherBahunturr.SelectTile();
        yield return new WaitUntil(() => manager.hexTileSelectHandler.selectedTiles.Count > 0);

        HexTile targetTile = manager.hexTileSelectHandler.selectedTile;
        HexTile spawnTile = HexTileManager.Instance.GetNearestTile(manager.GetPlayer().GetComponent<PlayerMove>().GetCurrentTile(), targetTile);
        EstherAnimationController skillController = InstantiateEsther(EstherBahunturrModel, spawnTile, targetTile).GetComponent<EstherAnimationController>();
        estherBahunturr.estherAnimationController = skillController;
        estherBahunturr.SpawnToGround(spawnTile);

        selectedEstherTiles = manager.hexTileSelectHandler.selectedTiles;
        SetEstherSkill(estherBahunturr);

        estherBahunturr.Execute();

        ResetEsther();


    }

    private void UseWayeSkill()
    {
        if (WayeSkillObject == null)
        {
            Debug.LogError("WayeSkillObject is not assigned!");
            return;
        }

        skillObj = Instantiate(WayeSkillObject);
        EstherSkill skill = skillObj.GetComponent<EstherSkill>();
        skill.Execute(); // 만약 즉시 발동형이라면
        estherValue = 0f;
        UpdateEstherBar();
        estherCancelButton.SetActive(false);
    }

}
