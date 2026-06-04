using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemPanelUI : TutorialPanelUI
{
    [SerializeField] private GameObject BattleItemUI;
    [SerializeField] private GameObject ChangeButtonArrow;

    [SerializeField] private GameObject UseButtonMask;
    [SerializeField] private GameObject ChangeButtonMask;
    [SerializeField] private GameObject NotPotionItemMask;
    [SerializeField] private GameObject ChangeOtherItemMask;

    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 1:
                BattleItemUI.SetActive(true);
                break;

            case 2:
                StartCoroutine(tutorialManager.TutorialTurnStart());
                break;

            case 3:
                Player.Instance.stats.GivePlayerDamage(new PlayerGetDamageInfo(60, true));
                NotPotionItemMask.SetActive(true);
                ChangeButtonMask.SetActive(true);
                tutorialManager.battleItemManager.battleItemUI.SelectSlotCursor(ItemType.Potion);
                tutorialManager.battleItemManager.ItemUsePotion += OnPotionItemUse;
                GetComponent<Image>().raycastTarget = false;
                break;

            case 4:
                tutorialManager.battleItemManager.battleItemUI.ResetSlotCursor();
                NotPotionItemMask.SetActive(false);
                ChangeButtonMask.SetActive(false);
                StartCoroutine(DelayedRaycastOn());
                break;

            case 5:
                break;

            case 6:
                StartCoroutine(tutorialManager.TutorialTurnStart());
                tutorialManager.battleItemManager.ItemUseGranade += OnGranadeItemUse;
                tutorialManager.battleItemManager.ItemChangeAction += OnItemChange;

                PrepareToUseDarkGranade();

                GetComponent<Image>().raycastTarget = false;
                break;

            case 7:
                GetComponent<Image>().raycastTarget = true;
                break;

        }

    }

    private void PrepareToUseDarkGranade()
    {
        UseButtonMask.SetActive(true);
        ChangeOtherItemMask.SetActive(true);
        tutorialManager.battleItemManager.OnSlotClickAction += ShowChangeButtonArrow;
    }


    private void OnPotionItemUse()
    {
        base.OnPointerDown(null);
        tutorialManager.battleItemManager.ItemUsePotion -= OnPotionItemUse;
    }

    private void OnGranadeItemUse()
    {
        base.OnPointerDown(null);
        tutorialManager.battleItemManager.ItemUseGranade -= OnGranadeItemUse;
    }

    private IEnumerator DelayedRaycastOn()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Image>().raycastTarget = true;
    }

    private void OnItemChange()
    {
        if (tutorialManager.battleItemManager.GetEquippedData(ItemType.Granade).granadeType == GranadeType.Dark)
        {
            UseButtonMask.SetActive(false);
            ChangeButtonArrow.SetActive(false);
        }
        tutorialManager.battleItemManager.ItemChangeAction -= OnItemChange;
    }

    private void ShowChangeButtonArrow()
    {
        ChangeButtonArrow.SetActive(true);
        tutorialManager.battleItemManager.OnSlotClickAction -= ShowChangeButtonArrow;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (index == 3) return;

        if (index == 6) return;

        base.OnPointerDown(eventData);
    }
}