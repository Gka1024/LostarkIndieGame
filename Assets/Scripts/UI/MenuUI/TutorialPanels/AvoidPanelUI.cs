using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AvoidPanelUI : TutorialPanelUI
{
    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 2:
                GameManager.Instance.OnBossDie += OnGameEnd;
                GetComponent<Image>().raycastTarget = false;
                break;
        }
    }

    private void OnGameEnd()
    {
        base.OnPointerDown(null);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (index == 2) return;

        base.OnPointerDown(eventData);
    }
}