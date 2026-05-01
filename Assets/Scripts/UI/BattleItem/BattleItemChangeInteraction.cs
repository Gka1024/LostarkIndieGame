using UnityEngine;
using UnityEngine.EventSystems;

public class BattleItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 이제 개별 변수 대신 ScriptableObject 데이터 하나만 가집니다.
    public BattleItemData itemData;
    public GameObject battleItemHighlight;

    // UI와 로직 매니저 참조
    private BattleItemUI battleItemUI;
    private BattleItemManager battleItemManager;

    private void Awake()
    {
        // 씬에 있는 UI와 매니저를 찾아둡니다.
        battleItemUI = FindFirstObjectByType<BattleItemUI>();
        battleItemManager = BattleItemManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData == null) return;

        battleItemHighlight.SetActive(true);
        // UI에게 데이터만 넘겨주면 이름과 설명을 알아서 표시합니다.
        battleItemUI.ShowChangeConfirmWindow(itemData);
        SoundManager.Instance.PlaySFX(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleItemHighlight.SetActive(false);
        battleItemUI.itemChangeWindow.SetActive(false); // 또는 RemoveChangeWindow 역할을 하는 함수
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null) return;

        // 1. 로직 매니저에게 아이템이 교체되었음을 알림 (데이터 전달)
        // Manager에서 아이템 타입에 맞는 슬롯에 데이터를 저장하게 합니다.
        battleItemManager.ChangeBattleItem(itemData);

        // 2. UI 업데이트
        // 해당 아이템의 아이콘으로 슬롯 이미지를 교체합니다.
        battleItemUI.UpdateSlotIcon(itemData.itemType, itemData.itemIcon);

        // 마무리
        battleItemHighlight.SetActive(false);
        battleItemUI.CloseAllChangeUI();
        battleItemUI.ShowDescWindow(false);
        battleItemManager.CancelItem();
    }
}