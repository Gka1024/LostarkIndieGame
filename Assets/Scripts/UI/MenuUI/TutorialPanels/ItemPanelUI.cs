using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanelUI : TutorialPanelUI
{
    [SerializeField] private GameObject BattleItemUI;

    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 1:
                BattleItemUI.SetActive(true);
                break;
            case 2:
                break;

        }

    }
}