using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemManager : MonoBehaviour
{
    public static BattleItemManager Instance { get; private set; }

    [Header("References")]
    public GameManager manager;
    public SkillManager skillManager;
    public HexTileSelectHandler hexTileSelectHandler;
    public PlayerStats playerStats;
    public BossController bossController;
    public PlayerAnimation playerAnimation;

    [Header("Current Selection")]
    [SerializeField] private BattleItemData currentItem;
    private Coroutine currentCoroutine;
    private bool isBattleItemUsing;
    private bool isCancelRequested;

    [Header("Equipped Items")]
    public ItemType currentItemType;
    public BattleItemData equippedPotion;
    public BattleItemData equippedGranade;
    public BattleItemData equippedSpecial;

    [Header("Item Count")]
    [SerializeField] private int PotionItemCountMAX;
    [SerializeField] private int GranadeItemCountMAX;
    [SerializeField] private int SpecialItemCountMAX;
    public int curPotionCount;
    public int curGranadeCount;
    public int curSpecialCount;

    [Header("Item Cooldown Settings")]
    [SerializeField] private int potionItemCooldown = 30;
    [SerializeField] private int granadeItemCooldown = 20;
    [SerializeField] private int specialItemCooldown = 40;

    [Header("Current Status")]
    public int curPotionCooldown;
    public int curGranadeCooldown;
    public int curSpecialCooldown;

    public int MaxPotionCooldown => potionItemCooldown;
    public int MaxGranadeCooldown => granadeItemCooldown;
    public int MaxSpecialCooldown => specialItemCooldown;

    [Header("UI Elements")]
    public BattleItemUI battleItemUI;

    [Header("Prefabs")]
    public GameObject itemPrefabCampFire;
    public GameObject itemPrefabScareCrow;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        SetItemCount();
        SetItemCooldown();
    }

    private void SetItemCount()
    {
        curPotionCount = PotionItemCountMAX;
        curGranadeCount = GranadeItemCountMAX;
        curSpecialCount = SpecialItemCountMAX;
        battleItemUI.UpdateCount(curPotionCount, curGranadeCount, curSpecialCount);
    }

    private void SetItemCooldown()
    {
        curPotionCooldown = 0;
        curGranadeCooldown = 0;
        curSpecialCooldown = 0;
        battleItemUI.RegisterMaxCooldown();
    }

    // ==== 아이템 클릭 및 선택 ====
    public void OnSlotClick(ItemType type)
    {
        if (isBattleItemUsing) return;

        // 1. 장착된 데이터 가져오기
        currentItem = GetEquippedData(type);
        currentItemType = type;
        if (currentItem == null) return;

        // 2. UI에게 "이 데이터로 설명창 띄워줘"라고 명령 (데이터 전달)
        battleItemUI.ShowControlButtons(true);
    }

    // ==== 아이템 데이터 교체 ====
    public void ChangeBattleItem(BattleItemData newData)
    {
        if (newData == null) return;

        // 타입에 따라 데이터 교체 (로직)
        switch (newData.itemType)
        {
            case ItemType.Potion: equippedPotion = newData; break;
            case ItemType.Granade: equippedGranade = newData; break;
            case ItemType.Special: equippedSpecial = newData; break;
        }

        // UI에게 "아이콘이랑 텍스트 갱신해"라고 명령 (연출)
        battleItemUI.UpdateSlotIcon(newData.itemType, newData.itemIcon);
        battleItemUI.UpdateDescWindow(newData);
    }

    // 장착된 데이터를 안전하게 가져오기 위한 함수
    public BattleItemData GetEquippedData(ItemType type)
    {
        return type switch
        {
            ItemType.Potion => equippedPotion,
            ItemType.Granade => equippedGranade,
            ItemType.Special => equippedSpecial,
            _ => null
        };
    }

    // ==== 아이템 사용 로직 ====
    public void UseItemButton()
    {
        if (currentItem == null || isBattleItemUsing) return;

        isBattleItemUsing = true;
        isCancelRequested = false;
        battleItemUI.ShowControlButtons(false, 0);
        battleItemUI.ResetSlotCursor();

        switch (currentItem.itemType)
        {
            case ItemType.Potion:
                if (curPotionCount > 0 && curPotionCooldown == 0)
                {
                    SetItemUsed(ItemType.Potion);
                    currentCoroutine = StartCoroutine(UsePotionSequence());
                }
                break;

            case ItemType.Granade:
                if (curGranadeCount > 0 && curGranadeCooldown == 0)
                {
                    SetItemUsed(ItemType.Granade);
                    currentCoroutine = StartCoroutine(UseThrowableSequence());
                }
                break;

            case ItemType.Special:
                if (curSpecialCount > 0 && curSpecialCooldown == 0)
                {
                    SetItemUsed(ItemType.Special);
                    currentCoroutine = StartCoroutine(UseSpecialSequence());
                }
                break;
        }
    }

    private IEnumerator UsePotionSequence()
    {
        battleItemUI.ShowControlButtons(false);
        yield return playerAnimation.DrinkPotion();

        switch (currentItem.potionType)
        {
            case PotionType.Heal:
                playerStats.Heal(30, true);
                VFXManager.Instance.PlayEffectAtPlayer(VFXID.Player_Heal, 1);
                break;

            case PotionType.Atropine:
                playerStats.AddAttackBuff(30, 0, 20);
                VFXManager.Instance.PlayEffectAtPlayer(VFXID.Player_Buff, 1);
                break;

            case PotionType.Shield:
                //playerStats.AddAttackBuff(30, 0, 20);
                VFXManager.Instance.PlayEffectAtPlayer(VFXID.Player_Shield, 1);
                break;

            case PotionType.TimeStop:
                //playerStats.AddAttackBuff(30, 0, 20);
                VFXManager.Instance.PlayEffectAtPlayer(VFXID.Player_Gold, 1);
                break;

            case PotionType.Identity:
                //playerStats.AddAttackBuff(30, 0, 20);
                VFXManager.Instance.PlayEffectAtPlayer(VFXID.Player_Identity, 1);
                break;
                // ... 나머지 물약 로직
        }
        EndItemTurn();
    }

    private IEnumerator UseThrowableSequence()
    {
        hexTileSelectHandler.StartSelectionItemGranades();

        yield return new WaitUntil(() => hexTileSelectHandler.isTileSelected || isCancelRequested);
        if (isCancelRequested) yield break;

        battleItemUI.ShowControlButtons(false);

        HexTile tile = hexTileSelectHandler.selectedTile;

        playerAnimation.ThrowItem(currentItem.granadeType, tile);
        StartCoroutine(PlaySmoke(0.5f, tile, currentItem.granadeType));


        // 보스 히트 판정 및 효과 적용
        if (HexTileManager.Instance.IsBossTile(hexTileSelectHandler.selectedTiles))
        {
            ApplyEffectToBoss();
        }

        EndItemTurn();
    }

    private IEnumerator PlaySmoke(float time, HexTile target, GranadeType type)
    {
        yield return new WaitForSeconds(time);

        int id = VFXID.BattleItem_Granade_Dark;

        switch (type)
        {
            case GranadeType.Clay:
                id = VFXID.BattleItem_Granade_Clay;
                break;

            case GranadeType.Corrosion:
                id = VFXID.BattleItem_Granade_Corrosion;
                break;

            case GranadeType.Dark:
                id = VFXID.BattleItem_Granade_Dark;
                break;

            case GranadeType.Destruction:
                id = VFXID.BattleItem_Granade_Destruction;
                break;

            case GranadeType.Electric:
                id = VFXID.BattleItem_Granade_Electric;
                break;

            case GranadeType.Flaming:
                id = VFXID.BattleItem_Granade_Flaiming;
                break;

            case GranadeType.Flashing:
                id = VFXID.BattleItem_Granade_Flashing;
                break;

            case GranadeType.Tornado:
                id = VFXID.BattleItem_Granade_Tornado;
                break;

        }

        VFXManager.Instance.PlayEffect(id, target, 0.4f);
    }

    private void ApplyEffectToBoss()
    {
        // 1. 데미지/무력화/파괴 적용
        bossController.GetBossDamageData(new BossDamageData(currentItem.damage, currentItem.stagger, currentItem.destruction, isTrueDamage: true));

        // 2. 디버프 인터페이스 적용 (중요!)
        if (currentItem.hasDebuff)
        {
            // Factory를 통해 인터페이스 객체 생성 및 전달
            BossBuff debuff = BossBuffFactory.CreateBuff(currentItem.buffID, 1, currentItem.buff_duration);
            bossController.AddBuff(debuff);
        }
    }

    private IEnumerator UseSpecialSequence()
    {
        hexTileSelectHandler.StartSelectionItemSpecial();

        yield return new WaitUntil(() => hexTileSelectHandler.isTileSelected || isCancelRequested);
        if (isCancelRequested) yield break;

        battleItemUI.ShowControlButtons(false);

        playerAnimation.UseSpecialItem(currentItem.specialType, hexTileSelectHandler.selectedTile);
        yield return new WaitForSeconds(1.0f);

        if (currentItem.specialType == SpecialType.CampFire)
            PlaceItem(hexTileSelectHandler.selectedTile, itemPrefabCampFire, 20);
        else if (currentItem.specialType == SpecialType.ScareCrow)
            PlaceItem(hexTileSelectHandler.selectedTile, itemPrefabScareCrow, 5);

        EndItemTurn();
    }

    // ==== 공통 관리 ===

    private void SetItemUsed(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Potion:
                curPotionCount--;
                curPotionCooldown = potionItemCooldown;
                break;

            case ItemType.Granade:
                curGranadeCount--;
                curGranadeCooldown = granadeItemCooldown;
                break;

            case ItemType.Special:
                curSpecialCount--;
                curSpecialCooldown = specialItemCooldown;
                break;

            default: break;
        }
    }

    private void EndItemTurn()
    {
        isBattleItemUsing = false;
        battleItemUI.UpdateCount(curPotionCount, curGranadeCount, curSpecialCount);
        battleItemUI.UpdateItemMask(curPotionCooldown, curGranadeCooldown, curSpecialCooldown);
        manager.EndPlayerTurn();
    }

    public void CancelItem()
    {
        isCancelRequested = true;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        hexTileSelectHandler.ResetVariables();
        isBattleItemUsing = false;
        battleItemUI.ShowControlButtons(false);
        currentItem = null;
    }

    public void ResetSelect()
    {
        battleItemUI.ItemCancelButtonClick();
    }

    private void PlaceItem(HexTile tile, GameObject obj, int duration)
    {
        Vector3 pos = new Vector3(tile.transform.position.x, 1.5f, tile.transform.position.z);
        GameObject item = Instantiate(obj, pos, Quaternion.identity);
        var placeable = item.GetComponent<BattleItemPlaceable>();
        placeable.RegisterHextile(tile);
        placeable.SetPlaceDuration(duration);
    }

    public void OnTurnEnd()
    {
        ReduceItemCooldown();
        battleItemUI.UpdateItemMask(curPotionCooldown, curGranadeCooldown, curSpecialCooldown);
    }

    private void ReduceItemCooldown()
    {
        if (curPotionCooldown > 0) curPotionCooldown--;
        if (curGranadeCooldown > 0) curGranadeCooldown--;
        if (curSpecialCooldown > 0) curSpecialCooldown--;
    }
}