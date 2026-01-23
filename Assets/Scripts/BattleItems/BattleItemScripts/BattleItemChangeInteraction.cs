using UnityEngine;
using UnityEngine.EventSystems;

public class BattleItemChangeInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public BattleItemManager battleItemManager;
    public GameObject battleItemHighlight;
    public GameObject itemDescription;

    public ItemType itemType;
    public PotionType potionType;
    public GranadeType grenadeType;
    public SpecialType specialType;

    public int itemID;

    public string itemName;
    public string itemDesc;

    void Awake()
    {
        battleItemManager = BattleItemManager.Instance;
        battleItemHighlight = FindHighlight();
        FindItemText();

    }

    private GameObject FindHighlight()
    {
        if (battleItemHighlight == null)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("BattleItemHighlight"))
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }

    private void FindItemText()
    {
        var itemData = BattleItemDataBase.Instance.GetItemDataByID(itemID);

        if (itemData != null)
        {
            itemName = itemData.Name;
            itemDesc = itemData.Description;
        }
        else
        {
            Debug.LogWarning($"아이템 ID {itemID} 데이터를 찾지 못했습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleItemHighlight.SetActive(true);
        battleItemManager.ChangeBattleItemWindow(itemName, itemDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleItemHighlight.SetActive(false);
        battleItemManager.RemoveChangeWindow();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (itemType)
        {
            case ItemType.Potion:
                battleItemManager.ChangeBattleItem(potionType);
                break;

            case ItemType.Granade:
                battleItemManager.ChangeBattleItem(grenadeType);
                break;

            case ItemType.Special:
                battleItemManager.ChangeBattleItem(specialType);
                break;

            default: break;
        }
        battleItemManager.SetTextBattleItemInteraction(itemType, itemName, itemDesc);
        battleItemHighlight.SetActive(false);
    }
}
