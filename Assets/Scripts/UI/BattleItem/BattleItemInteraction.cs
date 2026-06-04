using UnityEngine;
using UnityEngine.EventSystems;

public class BattleItemInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    public BattleItemUI battleItemUI;
    public BattleItemManager battleItemManager;

    [Header("Slot Setting")]
    public ItemType slotType;

    /// 마우스가 슬롯 위에 올라갔을 때 (하이라이트 + 설명창)
    public void OnPointerEnter(PointerEventData eventData)
    {
        battleItemUI.UserCursorOnItem();

        if (!TurnStateMachine.Instance.CanPlayerInteract) return;

        BattleItemData data = battleItemManager.GetEquippedData(slotType);
        if (data == null) return;

        battleItemUI.SetSlotHighlight(slotType, true);
        battleItemUI.UpdateDescWindow(data);
        SoundManager.Instance.PlaySFX(1);
    }

    /// 마우스가 슬롯을 벗어났을 때 (하이라이트 OFF + 설명창 OFF)
    public void OnPointerExit(PointerEventData eventData)
    {
        battleItemUI.SetSlotHighlight(slotType, false);
        battleItemUI.ShowDescWindow(false);
    }

    /// 슬롯을 클릭했을 때 (선택 커서 ON + 사용 대기 상태)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!TurnStateMachine.Instance.CanPlayerInteract) return;

        battleItemUI.SelectSlotCursor(slotType);

        battleItemManager.OnSlotClick(slotType);

    }

    public void BattleItemCursorOff()
    {
        battleItemUI.SetSlotCursor(slotType, false);
    }
}