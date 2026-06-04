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

    public EstherUI estherUI;

    public Boss boss;
    public Player player;

    [SerializeField] private bool isEstherUsing;
    public Action OnEstherSkillUse;

    // 에스더 스킬 오브젝트들
    public GameObject SkillObject_Thirain;
    public GameObject SkillObject_Waye;
    public GameObject SkillObject_Bahunturr;
    public GameObject SkillObject_Ninave;
    public GameObject SkillObject_Inanna;
    public GameObject SkillObject_Azena;
    public GameObject SkillObject_Shandi;
    public GameObject SkillObject_KadanAttack;
    public GameObject SkillObject_KadanShield;

    private GameObject skillObj;
    [SerializeField] private EstherSkill pendingEstherSkill;

    // 에스더 게이지 관련

    public float estherGainPerTurn = 14f;

    private const float MAX_ESTHER_VALUE = 350;
    [SerializeField] private float estherValue;

    // 에스더 스킬 타일 선택 관련
    public bool isEstherTileSelected { get; private set; }
    [SerializeField] private List<HexTile> selectedEstherTiles = new();

    // 에스더 캐릭터 관련
    public GameObject Model_Thirain;
    public GameObject Model_Bahuntur;
    public GameObject Model_Waye;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        estherUI.Init();
        estherUI.UpdateEstherBar(estherValue);
    }

    public void SetEstherValue(float value)
    {
        estherValue = Mathf.Clamp(value, 0, MAX_ESTHER_VALUE);
        estherUI.UpdateEstherBar(estherValue);
    }

    public void AddEstherValue(float value)
    {
        estherValue += value;
        estherValue = Mathf.Min(estherValue, MAX_ESTHER_VALUE);
        estherUI.UpdateEstherBar(estherValue);
    }

    public float GetMaxEstherValue()
    {
        return MAX_ESTHER_VALUE;
    }

    public void ClearEstherSkill()
    {
        Debug.Log("EstherClear");
        pendingEstherSkill = null;
    }

    public bool IsEstherFull()
    {
        return estherValue == MAX_ESTHER_VALUE;
    }

    public void MakeEstherFull()
    {
        AddEstherValue(MAX_ESTHER_VALUE);
    }

    public void UseEstherSkill(EstherType esther)
    {
        if (!IsEstherFull()) return;
        if (isEstherUsing) return;

        isEstherUsing = true;

        estherUI.estherCancelButton.SetActive(true);

        switch (esther)
        {
            case EstherType.Sillian:
                StartCoroutine(UseEstherSkill(SkillObject_Thirain, Model_Thirain));
                break;

            case EstherType.Waye:
                StartCoroutine(UseEstherSkill(SkillObject_Waye, Model_Waye));
                break;

            case EstherType.Bahunturr:
                StartCoroutine(UseEstherSkill(SkillObject_Bahunturr, Model_Bahuntur));
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
        Destroy(skillObj);
        isEstherUsing = false;
        isEstherTileSelected = false;
        selectedEstherTiles.Clear();
        GameManager.Instance.hexTileSelectHandler.CancelSelection();
        estherUI.estherCancelButton.SetActive(false);
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

        if(pendingEstherSkill.isFinished)
        {
            pendingEstherSkill.FinishSkill();
        }
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

    public void GivePlayerBuff(int duration)
    {
        foreach (HexTile tile in selectedEstherTiles)
        {
            if (player.move.GetCurrentTile() == tile)
            {
                player.state.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.ESTHER_BAHUNTUR, duration));
                return;
            }
        }
    }

    // ==== 이하는 구체적인 에스더 스킬의 사용입니다.

    private void ResetEsther()
    {
        estherValue = 0f;
        estherUI.UpdateEstherBar(estherValue);
        isEstherUsing = false;
        estherUI.estherCancelButton.SetActive(false);
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

    // 에스더 스킬을 통합해서 관리하는 메서드
    private IEnumerator UseEstherSkill(GameObject skillPrefab, GameObject modelPrefab)
    {
        // 1. 유효성 검사
        if (skillPrefab == null)
        {
            Debug.LogError("Skill Object is not assigned!");
            yield break;
        }

        // 2. 스킬 오브젝트 생성 및 초기화
        skillObj = Instantiate(skillPrefab);
        EstherSkill estherSkill = skillObj.GetComponent<EstherSkill>();

        estherSkill.estherManager = this;

        // 3. 타일 선택 대기
        estherSkill.SelectTile();
        yield return new WaitUntil(() => manager.hexTileSelectHandler.selectedTiles.Count > 0);

        // 4. 타겟/스폰 타일 계산 및 모델 생성
        HexTile targetTile = manager.hexTileSelectHandler.selectedTile;
        HexTile spawnTile = HexTileManager.Instance.GetNearestTile(Player.Instance.move.GetCurrentTile(), targetTile);

        EstherAnimationController skillController = InstantiateEsther(modelPrefab, spawnTile, targetTile).GetComponent<EstherAnimationController>();
        estherSkill.estherAnimationController = skillController;
        estherSkill.SpawnToGround(spawnTile);

        estherSkill.Init(spawnTile, skillController.gameObject);

        // 5. 실행 및 리셋
        selectedEstherTiles = new List<HexTile>(manager.hexTileSelectHandler.selectedTiles);
        SetEstherSkill(estherSkill);
        estherSkill.Execute(targetTile, selectedEstherTiles);
        OnEstherSkillUse?.Invoke();
        ResetEsther();
    }

}
