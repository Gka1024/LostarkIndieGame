using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class BattleItemManager : MonoBehaviour
{
    public GameManager manager;
    public static BattleItemManager Instance;

    // ==== 디버깅용
    public ItemType currentItemType;
    public PotionType currentPotionType;
    public GranadeType currentGranadeType;
    public SpecialType currentSpecialType;

    [SerializeField] private bool isItemSelected;
    [SerializeField] private bool isBattleItemUsing;
    [SerializeField] private bool isCancelRequested;

    // ==== 매니저 및 상태
    public SkillManager skillManager;
    public HexTileSelectHandler hexTileSelectHandler;
    public PlayerStats playerStats;
    public BossController bossController;
    //public BossStats bossStats;
    public PlayerAnimation playerAnimation;
    public GameObject UI;

    // ==== 아이템 게임오브젝트
    public BattleItemInteraction itemPotion, itemGranade, itemSpecial;
    public GameObject iconPotionHeal, iconPotionIdentity, iconPotionAtropine, iconPotionShield, iconPotionTimeStop;
    public GameObject iconThrowClay, iconThrowCorrosion, iconThrowDark, iconThrowDestruction, iconThrowElectric, iconThrowFlaming, iconThrowFlashing, iconThrowTornado;
    public GameObject iconSpecialPaperAmulet, iconSpecialScarecrow, iconSpecialHidingRobe, iconSpecialCampfire, iconSpecialMarchingFlag;
    public GameObject ItemSpawnPoint1, ItemSpawnPoint2, ItemSpawnPoint3;

    public GameObject itemUseButton;
    public GameObject itemCancelButton;
    public GameObject itemChangeButton;

    public GameObject itemChangePotion;
    public GameObject itemChangeGranade;
    public GameObject itemChangeSpecial;

    public GameObject itemChangeWindow;
    public TextMeshProUGUI itemChangeWindowName;
    public TextMeshProUGUI itemChangeWindowDescription;

    // ==== 아이템 프리팹
    public GameObject ItemPrefabCampFire;
    public GameObject ItemPrefabScareCrow;

    // ==== 아이템 설명창
    public GameObject ItemDescWindow;
    public TextMeshProUGUI ItemDescWindowName, ItemDescWindowDesc;

    // ==== 세부 변수
    public int itemPotionInterval = 40;
    public int itemGranadeInterval = 30;
    public int itemSpecialInterval = 60;

    public int potionItemCounter = 0;
    public int granadeItemCounter = 0;
    public int specialItemCounter = 0;

    private Coroutine currentCoroutine;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }
    }

    // ==== 아이템 상호작용

    private void ResetCurrentItem()
    {
        currentItemType = ItemType.Unset;
        currentPotionType = PotionType.Unset;
        currentGranadeType = GranadeType.Unset;
        currentSpecialType = SpecialType.Unset;
    }

    public void OnItemClick(GameObject obj)
    {
        ResetCurrentItem();
        ResetBattleItemCursor();

        BattleItemInteraction interaction = obj.GetComponent<BattleItemInteraction>();
        currentItemType = interaction.itemType;

        switch (currentItemType)
        {
            case ItemType.Potion:
                currentPotionType = interaction.potionType;
                break;
            case ItemType.Granade:
                currentGranadeType = interaction.granadeType;
                break;
            case ItemType.Special:
                currentSpecialType = interaction.specialType;
                break;
            case ItemType.Unset:
                Debug.LogError("ItemType is unset");
                break;
        }

        if (!isItemSelected)
        {
            isItemSelected = true;
        }
        else
        {
            ChangeDescWindow(interaction.itemName, interaction.itemDesc, true);
        }

        
        ShowButtonUI(true);
    }

    // ==== 버튼 상호작용

    public void UseItemButton()
    {
        UseItem();
    }

    public void CancelItemButton()
    {
        CancelItem(false);
    }

    public void ChangeItemButton()
    {
        ShowChangeBattleItemUI();
    }

    // ==== 턴 진행

    public void OnTurnEnd()
    {
        potionItemCounter = Mathf.Max(0, potionItemCounter - 1);
        granadeItemCounter = Mathf.Max(0, granadeItemCounter - 1);
        specialItemCounter = Mathf.Max(0, specialItemCounter - 1);
    }

    private void EndBattleItemTurn()
    {
        ShowButtonUI(false);
        isBattleItemUsing = false;
        manager.EndPlayerTurn();
    }

    // ==== UI 온오프

    public void ResetBattleItemCursor()
    {
        ItemSpawnPoint1.GetComponent<BattleItemInteraction>().BattleItemCursorOff();
        ItemSpawnPoint2.GetComponent<BattleItemInteraction>().BattleItemCursorOff();
        ItemSpawnPoint3.GetComponent<BattleItemInteraction>().BattleItemCursorOff();
    }

    private void ShowButtonUI(bool show)
    {
        itemChangeButton.SetActive(show);
        itemUseButton.SetActive(show);
        itemCancelButton.SetActive(show);
    }

    public void ShowChangeBattleItemUI(bool show = true)
    {
        ResetBattleItemCursor();
        ResetItemChangeUI();
        if (!show) return;
        switch (currentItemType)
        {
            case ItemType.Potion:
                itemChangePotion.SetActive(true);
                break;

            case ItemType.Granade:
                itemChangeGranade.SetActive(true);
                break;

            case ItemType.Special:
                itemChangeSpecial.SetActive(true);
                break;

            default: break;
        }
    }

    private void ResetItemChangeUI()
    {
        itemChangePotion.SetActive(false);
        itemChangeGranade.SetActive(false);
        itemChangeSpecial.SetActive(false);
    }

    private void ResetPotionIcon()
    {
        iconPotionHeal.SetActive(false);
        iconPotionIdentity.SetActive(false);
        iconPotionAtropine.SetActive(false);
        iconPotionShield.SetActive(false);
        iconPotionTimeStop.SetActive(false);
    }

    private void ResetGranadeIcon()
    {
        iconThrowClay.SetActive(false);
        iconThrowCorrosion.SetActive(false);
        iconThrowDark.SetActive(false);
        iconThrowDestruction.SetActive(false);
        iconThrowElectric.SetActive(false);
        iconThrowFlaming.SetActive(false);
        iconThrowFlashing.SetActive(false);
        iconThrowTornado.SetActive(false);
    }

    private void ResetSpecialItem()
    {
        iconSpecialPaperAmulet.SetActive(false);
        iconSpecialCampfire.SetActive(false);
        iconSpecialScarecrow.SetActive(false);
        iconSpecialHidingRobe.SetActive(false);
        iconSpecialMarchingFlag.SetActive(false);
    }

    public void RemoveChangeUI()
    {
        ShowButtonUI(false);
        RemoveChangeWindow();
        ShowDescWindow(false, true);
        itemChangePotion.SetActive(false);
        itemChangeGranade.SetActive(false);
        itemChangeSpecial.SetActive(false);
    }

    public void RemoveChangeWindow()
    {
        itemChangeWindow.SetActive(false);
    }

    // ==== 아이템 설명

    public void ShowDescWindow(bool show, bool ignoreItemSelected = false)
    {
        if (ignoreItemSelected)
        {
            ItemDescWindow.SetActive(show);
        }

        if (!isItemSelected)
        {
            ItemDescWindow.SetActive(show);
        }
    }

    public void ChangeDescWindow(string itemName, string itemDesc, bool ignoreItemSelected = false)
    {
        if (!ignoreItemSelected && isItemSelected) return;

        ItemDescWindowName.SetText(itemName);
        ItemDescWindowDesc.SetText(itemDesc);
        ShowDescWindow(true, true);
    }

    // ==== 아이템 사용

    public void UseItem()
    {
        if (isBattleItemUsing) return;

        isBattleItemUsing = true;
        isCancelRequested = false;
        isItemSelected = false;

        ShowDescWindow(false, true);
        ResetBattleItemCursor();

        switch (currentItemType)
        {
            case ItemType.Potion:
                if (potionItemCounter == 0) StartCoroutine(UsePotionCoroutine());
                break;

            case ItemType.Granade:
                currentCoroutine = StartCoroutine(UseGranadeItem());
                break;

            case ItemType.Special:
                currentCoroutine = StartCoroutine(UseSpecialItem());
                break;

            default:
                Debug.LogError("Invalid item type.");
                break;
        }
    }

    private void StartTileSelection(ItemType type)
    {
        switch (type)
        {
            case ItemType.Granade:
                hexTileSelectHandler.StartSelectionItemGranades();
                break;
            case ItemType.Special:
                hexTileSelectHandler.StartSelectionItemCampFire();
                break;
        }
    }

    // ==== 아이템 취소

    public void CancelItem(bool notifyTurnEnd = false)
    {
        isCancelRequested = true;
        isBattleItemUsing = false;
        isItemSelected = false;

        hexTileSelectHandler.ResetVariables();
        ResetBattleItemCursor();
        RemoveChangeUI();
        ShowDescWindow(false, true);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        ResetCurrentItem();
        ShowButtonUI(false);
        isCancelRequested = false;

        if (notifyTurnEnd)
        {
            manager.EndPlayerTurn();
        }
    }

    // ==== 아이템 교체 

    public void ChangeBattleItem(PotionType item)
    {
        currentPotionType = item;
        ResetPotionIcon();

        itemPotion.potionType = item;

        switch (currentPotionType)
        {
            case PotionType.Heal:
                iconPotionHeal.SetActive(true);
                break;

            case PotionType.Identity:
                iconPotionIdentity.SetActive(true);
                break;

            case PotionType.Atropine:
                iconPotionAtropine.SetActive(true);
                break;

            case PotionType.Shield:
                iconPotionShield.SetActive(true);
                break;

            case PotionType.TimeStop:
                iconPotionTimeStop.SetActive(true);
                break;

            default: break;
        }

        isItemSelected = false;
        RemoveChangeUI();
    }

    public void ChangeBattleItem(GranadeType item)
    {
        currentGranadeType = item;
        ResetGranadeIcon();

        itemGranade.granadeType = item;

        switch (currentGranadeType)
        {
            case GranadeType.Clay: // 점토 수류탄
                iconThrowClay.SetActive(true);
                break;

            case GranadeType.Corrosion: // 부식 수류탄
                iconThrowCorrosion.SetActive(true);
                break;

            case GranadeType.Dark: // 암흑 수류탄
                iconThrowDark.SetActive(true);
                break;

            case GranadeType.Destruction: // 파괴 수류탄(사실 폭탄이 맞음)
                iconThrowDestruction.SetActive(true);
                break;

            case GranadeType.Electric: // 전기 수류탄(이런게 있는지도 몰랐음)
                iconThrowElectric.SetActive(true);
                break;

            case GranadeType.Flaming: // 화염 수류탄
                iconThrowFlaming.SetActive(true);
                break;

            case GranadeType.Flashing: // 섬광 수류탄(얘도 몰랐음)
                iconThrowFlashing.SetActive(true);
                break;

            case GranadeType.Tornado: // 회오리 수류탄
                iconThrowTornado.SetActive(true);
                break;
                // 다른 수류탄 효과 추가
        }
        RemoveChangeUI();
    }

    public void ChangeBattleItem(SpecialType item)
    {
        currentSpecialType = item;
        ResetSpecialItem();

        itemSpecial.specialType = item;

        switch (currentSpecialType)
        {
            case SpecialType.CampFire: // 지속적으로 주변에 힐을 함
                iconSpecialCampfire.SetActive(true);
                break;

            case SpecialType.ScareCrow: // 보스의 어그로를 받음
                iconSpecialScarecrow.SetActive(true);
                break;

            case SpecialType.PaperAmulet: // 일정 상태이상을 해제함
                iconSpecialPaperAmulet.SetActive(true);
                break;

            case SpecialType.HidingRobe: // 보스에게 받는 데미지를 1회 무효화함
                iconSpecialHidingRobe.SetActive(true);
                break;

            case SpecialType.MarchingFlag: // 긴급 이동 쿨타임을 없앰
                iconSpecialMarchingFlag.SetActive(true);
                break;

                // 다른 특수 아이템도 여기에 추가
        }

        RemoveChangeUI();
    }

    public void ChangeBattleItemWindow(string name, string desc)
    {
        itemChangeWindowName.SetText(name);
        itemChangeWindowDescription.SetText(desc);
        itemChangeWindow.SetActive(true);
    }

    public void SetTextBattleItemInteraction(ItemType type, string name, string desc)
    {
        BattleItemInteraction battleItem = itemPotion; // 초기화용

        switch (type)
        {
            case ItemType.Potion:
                battleItem = itemPotion;
                break;

            case ItemType.Granade:
                battleItem = itemGranade;
                break;

            case ItemType.Special:
                battleItem = itemSpecial;
                break;

            default: break;
        }

        battleItem.SetTextName(name);
        battleItem.SetTextDesc(desc);

    }

    // ==== 물약 아이템

    private IEnumerator UsePotionCoroutine()
    {
        yield return playerAnimation.DrinkPotion();

        switch (currentPotionType)
        {
            case PotionType.Heal:
                playerStats.Heal(30, true);
                potionItemCounter = itemPotionInterval;
                break;

            case PotionType.Identity:
                playerStats.AddPlayerIdentity(PlayerStats.IDENTITY_MAX);
                break;

            case PotionType.Atropine:
                playerStats.GetPlayerDamage(new PlayerGetDamageInfo(50, true));
                playerStats.AddAttackBuff(30, 0, 20);
                break;

            case PotionType.Shield:
                playerStats.AddShield(60, 5);
                break;

            case PotionType.TimeStop:
                playerStats.UsingPotionItem(currentPotionType);
                QueueManager.Instance.Clear();
                break;

        }
        EndBattleItemTurn();
    }

    // ==== 수류탄/폭탄 아이템

    public IEnumerator UseGranadeItem()
    {
        StartTileSelection(ItemType.Granade);

        yield return new WaitUntil(() => hexTileSelectHandler.isTileSelected || isCancelRequested);
        if (isCancelRequested) yield break;

        playerAnimation.ThrowItem(currentGranadeType, hexTileSelectHandler.selectedTile);

        if (HexTileManager.Instance.IsBossTile(hexTileSelectHandler.selectedTiles))
        {
            ApplyGranadeEffects();
        }

        EndBattleItemTurn();
        granadeItemCounter = itemGranadeInterval;
    }

    public void ApplyGranadeEffects()
    {
        BattleItemGranadeData data = iconThrowClay.GetComponent<BattleItemGranadeData>(); // 초기화용입니다. 

        switch (currentGranadeType)
        {
            case GranadeType.Clay: // 점토 수류탄
                data = iconThrowClay.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Corrosion: // 부식 수류탄
                data = iconThrowCorrosion.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Dark: // 암흑 수류탄
                data = iconThrowDark.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Destruction: // 파괴 수류탄(사실 폭탄이 맞음)
                data = iconThrowDestruction.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Electric: // 전기 수류탄(이런게 있는지도 몰랐음)
                data = iconThrowElectric.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Flaming: // 화염 수류탄
                data = iconThrowFlaming.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Flashing: // 섬광 수류탄(얘도 몰랐음)
                data = iconThrowFlashing.GetComponent<BattleItemGranadeData>();
                break;

            case GranadeType.Tornado: // 회오리 수류탄
                data = iconThrowTornado.GetComponent<BattleItemGranadeData>();
                break;
                // 다른 수류탄 효과 추가
        }

        ApplyOnBoss(data.damage, data.stagger, data.destruction, data.debuffType, data.effectValue, data.duration);
    }

    private void ApplyOnBoss(float damage, float stagger, int destruction, DebuffType debuffType, float effectValue, int duration)
    {
        bossController.GetBossDamage(new BossDamageInfo(damage, stagger, destruction));

        if (duration != 0) bossController.GetBossDebuff(new BossDebuff(debuffType, effectValue, duration));
    }

    // ==== 특수 아이템

    public IEnumerator UseSpecialItem()
    {
        StartTileSelection(ItemType.Special);

        yield return new WaitUntil(() => hexTileSelectHandler.isTileSelected || isCancelRequested);
        if (isCancelRequested) yield break;

        playerAnimation.UseSpecialItem(currentSpecialType, hexTileSelectHandler.selectedTile);
        yield return new WaitForSeconds(1.0f);

        switch (currentSpecialType)
        {
            case SpecialType.CampFire: // 지속적으로 주변에 힐을 함
                PlaceItem(hexTileSelectHandler.selectedTile, ItemPrefabCampFire, 20);
                break;

            case SpecialType.ScareCrow: // 보스의 어그로를 받음
                PlaceItem(hexTileSelectHandler.selectedTile, ItemPrefabScareCrow, 5);
                break;

            case SpecialType.PaperAmulet: // 일정 상태이상을 해제함
                playerStats.UsingSpecialItem(currentSpecialType);
                break;

            case SpecialType.HidingRobe: // 보스에게 받는 데미지를 1회 무효화함
                playerStats.UsingSpecialItem(currentSpecialType);
                break;

            case SpecialType.MarchingFlag: // 긴급 이동 쿨타임을 없앰
                playerStats.UsingSpecialItem(currentSpecialType);
                break;

                // 다른 특수 아이템도 여기에 추가
        }
        EndBattleItemTurn();
        specialItemCounter = itemSpecialInterval;
    }

    private void PlaceItem(HexTile tile, GameObject obj, int duration)
    {
        Vector3 itemPlacePosition = new(tile.transform.position.x, 1.5f, tile.transform.position.z);
        GameObject placingItem = Instantiate(obj, itemPlacePosition, quaternion.identity);
        placingItem.GetComponent<BattleItemPlaceable>().RegisterHextile(tile);
        placingItem.GetComponent<BattleItemPlaceable>().SetPlaceDuration(duration);
    }
}