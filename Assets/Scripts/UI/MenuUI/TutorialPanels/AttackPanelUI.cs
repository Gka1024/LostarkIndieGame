using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackPanelUI : TutorialPanelUI
{
    [SerializeField] private GameObject CardUI;
    [SerializeField] private GameObject CardtripodUI;
    [SerializeField] private GameObject BossUI;
    [SerializeField] private GameObject PlayerUI;

    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 1:
                CardUI.SetActive(true);
                BossUI.SetActive(true);
                PlayerUI.SetActive(true);
                tutorialManager.objectClickHandler.SetClickAvailable(false);
                break;

            case 2:
                tutorialManager.cardManager.GiveSpecificCard(111);
                StartCoroutine(PrepareCardUseStage());
                break;

            case 3:
                CardtripodUI.SetActive(false);
                tutorialManager.objectClickHandler.SetClickAvailable(true);
                break;

            case 4:
                CardtripodUI.SetActive(true);
                needLoopAgain = true;
                StartCoroutine(tutorialManager.TutorialTurnStart());
                GameManager.Instance.OnTurnEnd += OnTurnEnd;
                break;

            case 5:
                needLoopAgain = false;
                break;

        }
    }

    private IEnumerator PrepareCardUseStage()
    {
        yield return new WaitForSeconds(0.3f);
        CardManager cardManager = tutorialManager.cardManager;

        if (cardManager == null)
        {
            Debug.LogError("CardManager가 할당되지 않았습니다!");
            yield return 0;
        }

        cardManager.CardUseAction -= OnCardUseTutorial;
        cardManager.CardUseAction += OnCardUseTutorial;
    }

    private void OnCardUseTutorial()
    {
        tutorialManager.cardManager.CardUseAction -= OnCardUseTutorial;
        base.OnPointerDown(null);
    }

    private void OnTurnEnd()
    {
        base.OnPointerDown(null);
        GameManager.Instance.OnTurnEnd -= OnTurnEnd;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (index == 2) return;

        if (index == 4) return;

        base.OnPointerDown(eventData);
    }
}