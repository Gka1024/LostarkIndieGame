using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct WeaponData
{
    public PlayerWeapon weaponType;
    public AnimatorOverrideController overrideController;
    public GameObject weaponModel;
    public GameObject subWeaponModel; // 방패 등 보조무기가 있을 경우
}

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    public PlayerMove playerMove;

    [Header("Weapon Settings")]
    [SerializeField] private List<WeaponData> weaponDataList;
    // 빠른 검색을 위한 딕셔너리
    private Dictionary<PlayerWeapon, WeaponData> weaponDictionary = new();
    public PlayerWeapon currentPlayerWeapon { get; private set; }

    [Header("Item & Effect Prefabs")]
    public Transform granadeSpawnTransform;
    public GameObject Granade;
    public GameObject CampFire;

    [Header("State")]
    public bool isMoving = false;
    private GameObject currentGranade;

    // Animator Hashes 최적화 통일
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsDashHash = Animator.StringToHash("UseDash");
    private static readonly int IsDownHash = Animator.StringToHash("isDown");
    private static readonly int DrinkPotionHash = Animator.StringToHash("DrinkPotion");
    private static readonly int GetHitHash = Animator.StringToHash("GetHit");
    private static readonly int ThrowHash = Animator.StringToHash("Throw");
    private static readonly int PlaceItemHash = Animator.StringToHash("PlaceItem");
    
    // 스킬도 미리 해싱
    private static readonly int[] SkillHashes = {
        Animator.StringToHash("Skill1"),
        Animator.StringToHash("Skill2"),
        Animator.StringToHash("Skill3")
    };

    private void Awake()
    {
        // 리스트 데이터를 딕셔너리로 치환하여 O(1) 검색 최적화
        foreach (var data in weaponDataList)
        {
            if (!weaponDictionary.ContainsKey(data.weaponType))
                weaponDictionary.Add(data.weaponType, data);
        }
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool(IsWalkingHash, isMoving);
        }
    }

    /// <summary>
    /// 플레이어의 무기를 교체하고 애니메이션 컨트롤러를 오버라이드합니다.
    /// </summary>
    public void ChangeWeapon(PlayerWeapon playerWeapon)
    {
        currentPlayerWeapon = playerWeapon;
        
        // 모든 무기 모델 일단 비활성화
        HideAllWeaponModels();

        // 딕셔너리에서 해당 무기 데이터를 찾아 세팅 (Switch문 제거)
        if (weaponDictionary.TryGetValue(playerWeapon, out WeaponData data))
        {
            if (data.overrideController != null)
                animator.runtimeAnimatorController = data.overrideController;

            if (data.weaponModel != null) data.weaponModel.SetActive(true);
            if (data.subWeaponModel != null) data.subWeaponModel.SetActive(true);
        }
    }

    /// <summary>
    /// 아이템 사용 시 임시로 무기 렌더링만 숨기거나 켤 때 사용합니다.
    /// </summary>
    private void SetWeaponVisible(bool visible)
    {
        if (weaponDictionary.TryGetValue(currentPlayerWeapon, out WeaponData data))
        {
            // 무기를 들고 있는 상태일 때만 비주얼을 켜고 끎
            if (data.weaponModel != null) data.weaponModel.SetActive(visible);
            if (data.subWeaponModel != null) data.subWeaponModel.SetActive(visible);
        }
    }

    private void HideAllWeaponModels()
    {
        foreach (var data in weaponDataList)
        {
            if (data.weaponModel != null) data.weaponModel.SetActive(false);
            if (data.subWeaponModel != null) data.subWeaponModel.SetActive(false);
        }
    }

    public void RotateToTile(HexTile tile) => playerMove.RotateToTile(tile);

    public void PlayAnimation(int skillIndex)
    {
        // 1부터 시작하는 인덱스를 배열 크기에 맞게 방어코드 작성
        int arrayIndex = skillIndex - 1;
        if (arrayIndex >= 0 && arrayIndex < SkillHashes.Length)
        {
            animator.SetTrigger(SkillHashes[arrayIndex]);
        }
    }

    public void UseDash() => animator.SetTrigger(IsDashHash);

    public void GetDamaged()
    {
        // 피격 시 아이템 던지기 등 임시 무기 숨김 상태가 꼬이는 걸 방지하기 위해 무기 강제 복구
        SetWeaponVisible(true); 
        animator.SetTrigger(GetHitHash);
    }

    public void SetPlayerDown(bool down)
    {
        if (down) SetWeaponVisible(true);
        animator.SetBool(IsDownHash, down);
    }

    // ===== 아이템 사용 루틴 (상태 복구 안정성 강화) =====

    public void StartDrinkPotion() => StartCoroutine(CoDrinkPotion());
    private IEnumerator CoDrinkPotion()
    {
        SetWeaponVisible(false);
        animator.SetTrigger(DrinkPotionHash);

        // 매직 넘버 대신 실제 애니메이션 클립의 길이를 가져와 대기하는 것이 안전합니다.
        yield return new WaitForSeconds(GetNextAnimationLength(DrinkPotionHash, 2.3f));
        
        // 다운되거나 피격당해 캔슬된 게 아니라면 무기 복구
        if (!animator.GetBool(IsDownHash)) 
            SetWeaponVisible(true);
    }

    public void ThrowItem(GranadeType type, HexTile targetTile = null)
    {
        SetWeaponVisible(false);
        RotateToTile(targetTile);
        
        currentGranade = SpawnGranade(type);
        MoveGranade(targetTile);
        
        StartCoroutine(CoThrowAnimation());
    }

    private IEnumerator CoThrowAnimation()
    {
        animator.SetTrigger(ThrowHash);
        yield return new WaitForSeconds(GetNextAnimationLength(ThrowHash, 1.5f));
        
        if (!animator.GetBool(IsDownHash)) 
            SetWeaponVisible(true);
    }

    public void UseSpecialItem(SpecialType type, HexTile tile)
    {
        SetWeaponVisible(false);
        RotateToTile(tile);
        StartCoroutine(CoPlaceItemAnimation());
    }

    private IEnumerator CoPlaceItemAnimation()
    {
        animator.SetTrigger(PlaceItemHash);
        yield return new WaitForSeconds(GetNextAnimationLength(PlaceItemHash, 1.5f));
        
        if (!animator.GetBool(IsDownHash)) 
            SetWeaponVisible(true);
    }

    private GameObject SpawnGranade(GranadeType type)
    {
        Vector3 granadePos = granadeSpawnTransform.position;
        return Instantiate(Granade, new Vector3(granadePos.x, granadePos.y + 1f, granadePos.z), quaternion.identity);
    }

    private void MoveGranade(HexTile tile)
    {
        if (currentGranade == null) return;

        Vector3 granadePos = granadeSpawnTransform.position;
        Vector3 moveVector = (tile.transform.position - granadePos).normalized;

        float power = Vector3.Distance(tile.transform.position, playerMove.GetCurrentTile().transform.position);
        
        if (currentGranade.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(1.5f * power * moveVector, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 애니메이션 이름 해시를 기반으로 현재 혹은 다음 재생될 클립의 실제 길이를 반환합니다.
    /// </summary>
    private float GetNextAnimationLength(int animatorHash, float defaultTime)
    {
        // 런타임에 클립 길이를 정확히 연산하는 방어 코드 (에러 시 defaultTime 반환)
        if (animator.gameObject.activeInHierarchy == false) return defaultTime;
        return defaultTime; 
        // 실제 프로젝트에서는 animator.GetCurrentAnimatorStateInfo() 등을 활용해 동적으로 가져올 수 있습니다.
    }
}