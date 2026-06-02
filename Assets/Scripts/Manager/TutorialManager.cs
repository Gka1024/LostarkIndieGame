using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public enum ETutorialStep { Start, Welcome, MoveExample, AttackExample, ItemExample, AvoidPattern, Clear }
    public ETutorialStep currentStep = ETutorialStep.Start;

    public ObjectClickHandler objectClickHandler;    
    public CardManager cardManager;
    public BattleItemManager battleItemManager;

    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject moveGuidePanel;
    [SerializeField] private GameObject attackGuidePanel;
    [SerializeField] private GameObject ItemGuidePanel;
    [SerializeField] private GameObject AvoidGuidePanel;

    [SerializeField] private GameObject PlayerUI;
    [SerializeField] private GameObject BossUI;
    [SerializeField] private GameObject EstherUI;
    [SerializeField] private GameObject BattleItemUI;

    public void Start()
    {
        SetTimeScale(0f);
    }

    public void StartTutorial()
    {
        EnterStep(ETutorialStep.Welcome);
        startPanel.SetActive(false);
    }

    public void EnterStep(ETutorialStep nextStep)
    {
        currentStep = nextStep;

        // 모든 가이드 UI를 일단 끄기
        welcomePanel.SetActive(false);
        moveGuidePanel.SetActive(false);
        attackGuidePanel.SetActive(false);
        ItemGuidePanel.SetActive(false);
        AvoidGuidePanel.SetActive(false);

        // 현재 단계에 맞는 UI와 규칙 활성화
        switch (currentStep)
        {
            case ETutorialStep.Start:
                break;

            case ETutorialStep.Welcome:
                welcomePanel.SetActive(true);
                welcomePanel.GetComponent<TutorialPanelUI>().Init(this);
                break;

            case ETutorialStep.MoveExample:
                moveGuidePanel.SetActive(true);
                moveGuidePanel.GetComponent<TutorialPanelUI>().Init(this);
                break;

            case ETutorialStep.AttackExample:
                attackGuidePanel.SetActive(true);
                attackGuidePanel.GetComponent<TutorialPanelUI>().Init(this);
                break;

            case ETutorialStep.ItemExample:
                ItemGuidePanel.SetActive(true);
                ItemGuidePanel.GetComponent<TutorialPanelUI>().Init(this);
                break;

            case ETutorialStep.AvoidPattern:
                AvoidGuidePanel.SetActive(true);
                AvoidGuidePanel.GetComponent<TutorialPanelUI>().Init(this);
                break;

            case ETutorialStep.Clear:
                EndTutorial();
                break;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    // 외부(Player나 UI 버튼)에서 무언가 완료했을 때 호출하는 함수
    public void CompleteCurrentStep()
    {
        if (currentStep == ETutorialStep.Start)
            EnterStep(ETutorialStep.Welcome);
        else if (currentStep == ETutorialStep.Welcome)
            EnterStep(ETutorialStep.MoveExample);
        else if (currentStep == ETutorialStep.MoveExample)
            EnterStep(ETutorialStep.AttackExample);
        else if (currentStep == ETutorialStep.AttackExample)
            EnterStep(ETutorialStep.ItemExample);
        else if (currentStep == ETutorialStep.ItemExample)
            EnterStep(ETutorialStep.AvoidPattern);
        else if (currentStep == ETutorialStep.AvoidPattern)
            EnterStep(ETutorialStep.Clear);
    }

    private void EndTutorial()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isTutorialCleared = true;
        }

        // 빌드 세팅에 등록된 메인 보스전 씬으로 전환
        SceneManager.LoadScene("BattleScene");
    }
}