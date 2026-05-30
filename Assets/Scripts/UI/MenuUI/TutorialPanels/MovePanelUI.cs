using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePanelUI : TutorialPanelUI
{
    // 에디터에서 플레이어 오브젝트를 할당하거나, 코드에서 Find해옵니다.
    [SerializeField] private GameObject playerObject;

    protected override void OnPanelApplied(int index)
    {
        switch (index)
        {
            case 2:
                tutorialManager.SetTimeScale(1f);
                PreparePlayerClickStage();
                break;
        }
    }

    private void PreparePlayerClickStage()
    {
        ObjectClickHandler objectClickHandler = tutorialManager.objectClickHandler;
        objectClickHandler.isClickAvailable = true;

        if (objectClickHandler == null)
        {
            Debug.LogError("ObjectClickHandler가 할당되지 않았습니다!");
            return;
        }

        // 혹시 모를 중복 구독 방지 후 이벤트 연결
        objectClickHandler.OnPlayerClicked -= OnPlayerClickedInsideTutorial;
        objectClickHandler.OnPlayerClicked += OnPlayerClickedInsideTutorial;
    }

    private void OnPlayerClickedInsideTutorial()
    {
        // 목적을 달성했으므로 이벤트를 해제합니다.
        tutorialManager.objectClickHandler.OnPlayerClicked -= OnPlayerClickedInsideTutorial;

        // 부모의 OnPointerDown을 호출하여 인덱스를 증가시키고 다음 패널(Index 4)을 엽니다.
        base.OnPointerDown(null);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 인덱스가 3일 때는 패널 자체를 클릭해서 넘어가는 것을 막음
        if (index == 2) return;

        base.OnPointerDown(eventData);
    }
}