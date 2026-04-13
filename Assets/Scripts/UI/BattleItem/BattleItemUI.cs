using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemUI : MonoBehaviour
{
    public BattleItemManager battleItemManager;
    

    [Header("Item Description UI")]
    public GameObject itemDescWindow;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;

    [Header("Control Buttons")]
    public GameObject itemUseButton;
    public GameObject itemCancelButton;
    public GameObject itemChangeButton;

    [Header("Selection Windows (Change UI)")]
    public GameObject itemChangeWindow; // 아이템 교체 확인 팝업
    public TextMeshProUGUI changeWindowName;
    public TextMeshProUGUI changeWindowDesc;

    public GameObject itemChangePotionPanel;
    public GameObject itemChangeGranadePanel;
    public GameObject itemChangeSpecialPanel;

    [Header("Item Slots Icons")]
    public Image iconPotion;
    public Image iconGranade;
    public Image iconSpecial;

    [Header("Item Highlights")]
    public GameObject[] highlightObjects;
    public GameObject[] cursorObjects;

    // --- 설명창 제어 ---
    public void UpdateDescWindow(BattleItemData data)
    {
        if (data == null) return;

        // 1. SO에 저장된 ID로 JSON 데이터베이스에서 정보를 찾아옴
        ItemData jsonTextData = BattleItemDataBase.Instance.GetItemDataByID(data.itemID);

        if (jsonTextData != null)
        {
            // 2. 찾은 이름과 설명을 UI에 적용
            itemNameText.SetText(jsonTextData.Name);
            itemDescText.SetText(jsonTextData.Description);
        }
        else
        {
            // ID가 잘못되었을 경우를 대비한 방어 코드
            itemNameText.SetText("Unknown Item");
            itemDescText.SetText($"ID {data.itemID}를 찾을 수 없습니다.");
        }

        itemDescWindow.SetActive(true);
    }
    public void ShowDescWindow(bool show)
    {
        itemDescWindow.SetActive(show);
    }

    // --- 버튼 UI 제어 ---
    public void ShowControlButtons(bool show)
    {
        itemUseButton.SetActive(show);
        itemCancelButton.SetActive(show);
        itemChangeButton.SetActive(show);
    }

    public void ShowControlButtons(bool show, int index)
    {
        switch (index)
        {
            case 0: itemUseButton.SetActive(show); break;
            case 1: itemCancelButton.SetActive(show); break;
            case 2: itemChangeButton.SetActive(show); break;
            default: break;
        }
    }

    public void ItemCancelButtonClick()
    {
        ShowControlButtons(false);
        battleItemManager.CancelItem();
        CloseAllChangeUI();
        ResetSlotCursor();
    }

    // --- 아이템 교체(Change) 관련 UI ---
    public void OpenChangePanel()
    {
        OpenChangePanel(battleItemManager.currentItemType);
        ResetSlotCursor();
    }

    public void OpenChangePanel(ItemType type)
    {
        // 모든 패널 일단 끄기
        itemChangePotionPanel.SetActive(false);
        itemChangeGranadePanel.SetActive(false);
        itemChangeSpecialPanel.SetActive(false);

        // 선택한 타입만 켜기
        switch (type)
        {
            case ItemType.Potion: itemChangePotionPanel.SetActive(true); break;
            case ItemType.Granade: itemChangeGranadePanel.SetActive(true); break;
            case ItemType.Special: itemChangeSpecialPanel.SetActive(true); break;
        }
    }

    public void ShowChangeConfirmWindow(BattleItemData soData)
    {
        ItemData jsonTextData = BattleItemDataBase.Instance.GetItemDataByID(soData.itemID);
        if (jsonTextData != null)
        {
            changeWindowName.SetText(jsonTextData.Name);
            changeWindowDesc.SetText(jsonTextData.Description);
            itemChangeWindow.SetActive(true);
        }
    }

    public void CloseAllChangeUI()
    {
        itemChangePotionPanel.SetActive(false);
        itemChangeGranadePanel.SetActive(false);
        itemChangeSpecialPanel.SetActive(false);
        itemChangeWindow.SetActive(false);
    }

    public void SetSlotHighlight(ItemType type, bool show)
    {
        highlightObjects[(int)type].SetActive(show);
    }

    public void SelectSlotCursor(ItemType type)
    {
        // 모든 커서 끄고 선택한 것만 켜기
        ResetSlotCursor();
        cursorObjects[(int)type].SetActive(true);
    }

    public void SetSlotCursor(ItemType type, bool show)
    {
        cursorObjects[(int)type].SetActive(show);
    }

    public void ResetSlotCursor()
    {
        foreach (var obj in cursorObjects) obj.SetActive(false);
    }

    // --- 아이콘 업데이트 (아이템 교체 완료 시 호출) ---
    public void UpdateSlotIcon(ItemType type, Sprite newIcon)
    {
        switch (type)
        {
            case ItemType.Potion: iconPotion.sprite = newIcon; break;
            case ItemType.Granade: iconGranade.sprite = newIcon; break;
            case ItemType.Special: iconSpecial.sprite = newIcon; break;
        }
    }
}