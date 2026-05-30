using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TutorialPanelUI : MonoBehaviour, IPointerDownHandler
{
    public TutorialManager tutorialManager;
    public GameObject button;

    [SerializeField] protected int index;
    [SerializeField] private GameObject[] panelsObject;

    public void Init(TutorialManager tutorialManager)
    {
        index = 0;
        this.tutorialManager = tutorialManager;
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

    protected abstract void OnPanelApplied(int index);

}