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
                StartCoroutine(PreparePlayerClickStage());
                break;
            case 3:
                tutorialManager.SetTimeScale(0f);
                tutorialManager.objectClickHandler.SetClickAvailable(false);
                break;
            case 4:
                tutorialManager.SetTimeScale(1f);
                StartCoroutine(PrepareTileClickStage());
                break;

            default: break;
        }
    }

    private IEnumerator PreparePlayerClickStage()
    {
        yield return new WaitForSeconds(0.3f);
        ObjectClickHandler objectClickHandler = tutorialManager.objectClickHandler;
        objectClickHandler.SetClickAvailable(true);

        if (objectClickHandler == null)
        {
            Debug.LogError("ObjectClickHandler가 할당되지 않았습니다!");
            yield return 0;
        }

        objectClickHandler.OnPlayerClicked -= OnPlayerClickedInsideTutorial;
        objectClickHandler.OnPlayerClicked += OnPlayerClickedInsideTutorial;
    }

    private void OnPlayerClickedInsideTutorial()
    {
        tutorialManager.objectClickHandler.OnPlayerClicked -= OnPlayerClickedInsideTutorial;
        base.OnPointerDown(null);
    }

    private IEnumerator PrepareTileClickStage()
    {
        yield return new WaitForSeconds(0.3f);
        ObjectClickHandler objectClickHandler = tutorialManager.objectClickHandler;
        objectClickHandler.SetClickAvailable(true);

        if (objectClickHandler == null)
        {
            Debug.LogError("ObjectClickHandler가 할당되지 않았습니다!");
            yield return 0;
        }

        objectClickHandler.OnTileClicked -= OntileClickedInsideTutorial;
        objectClickHandler.OnTileClicked += OntileClickedInsideTutorial;
    }

    private void OntileClickedInsideTutorial()
    {
        tutorialManager.objectClickHandler.OnTileClicked -= OntileClickedInsideTutorial;
        base.OnPointerDown(null);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 인덱스가 3일 때는 패널 자체를 클릭해서 넘어가는 것을 막음
        if (index == 2) return;
        if (index == 4) return;

        base.OnPointerDown(eventData);
    }
}