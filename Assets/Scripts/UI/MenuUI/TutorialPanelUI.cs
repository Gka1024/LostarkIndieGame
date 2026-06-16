using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TutorialPanelUI : MonoBehaviour, IPointerDownHandler
{
    public TutorialManager tutorialManager;
    public GameObject button;

    protected bool needLoopAgain = false;

    [SerializeField] protected int index;
    [SerializeField] private GameObject[] panelsObject;

    public void Init(TutorialManager tutorialManager)
    {
        index = 0;
        needLoopAgain = false;
        this.tutorialManager = tutorialManager;
        GameManager.Instance.turnStateMachine.OnLoopEnd += CheckLoopAgain;
    }

    public void ReviveButton()
    {
        GameManager.Instance.Revive();
    }

    public void ResetButton()
    {
        GameManager.Instance.RestartCurrentScene();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        index++;

        if (index >= panelsObject.Count())
        {
            tutorialManager.CompleteCurrentStep();
            return;
        }

        OpenPanel(index);

    }

    private void OpenPanel(int index)
    {
        ResetAllPanels();
        OnPanelApplied(index);
        panelsObject[index].SetActive(true);
    }

    private void ResetAllPanels()
    {
        foreach (var panel in panelsObject)
        {
            panel.SetActive(false);
        }
    }

    public void ResetEvent()
    {
        GameManager.Instance.turnStateMachine.OnLoopEnd -= CheckLoopAgain;
    }

    protected void CheckLoopAgain()
    {
        if (needLoopAgain)
        {
            StartCoroutine(tutorialManager.TutorialTurnStart());
        }
    }

    protected abstract void OnPanelApplied(int index);

}