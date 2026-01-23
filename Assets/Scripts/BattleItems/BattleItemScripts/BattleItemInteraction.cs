using UnityEngine;
using UnityEngine.EventSystems;

public class BattleItemInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public BattleItemManager BattleItemManager;
    public GameObject BattleItemHighlight;
    public GameObject BattleItemCursor;

    public ItemType itemType;
    public PotionType potionType;
    public GranadeType granadeType;
    public SpecialType specialType;

    public string itemName, itemDesc;

    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleItemHighlight.SetActive(true);
        BattleItemManager.ChangeDescWindow(itemName, itemDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleItemHighlight.SetActive(false);
        BattleItemManager.ShowDescWindow(false);
    }

    public void BattleItemCursorOff()
    {
        BattleItemCursor.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleItemManager.OnItemClick(this.gameObject);
        BattleItemCursor.SetActive(true);
    }

    public void SetTextName(string name)
    {
        itemName = name;
    }

    public void SetTextDesc(string desc)
    {
        itemDesc = desc;
    }
}
