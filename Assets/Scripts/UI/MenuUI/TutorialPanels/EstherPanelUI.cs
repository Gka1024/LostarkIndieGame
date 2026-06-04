using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EstherPanelUI : TutorialPanelUI
{
    [SerializeField] private GameObject EstherUI;
    [SerializeField] private GameObject EstherMaskUI;
    [SerializeField] private GameObject EstherPointerUI;

    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 1:
                EstherUI.SetActive(true);
                EstherMaskUI.SetActive(true);
                GameManager.Instance.turnStateMachine.isLoopOnce = false;
                break;

            case 2:
                break;

            case 3:
                GameManager.Instance.turnStateMachine.StartTurnLoop();
                EstherPointerUI.SetActive(true);
                tutorialManager.estherManager.OnEstherSkillUse += OnEstherSkillUse;
                tutorialManager.estherManager.MakeEstherFull();
                GetComponent<Image>().raycastTarget = false;
                break;

            case 4:

                break;

        }
    }

    private void OnEstherSkillUse()
    {
        base.OnPointerDown(null);
        EstherPointerUI.SetActive(false);
        tutorialManager.estherManager.OnEstherSkillUse -= OnEstherSkillUse;
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (index == 3) return;

        base.OnPointerDown(eventData);
    }
}