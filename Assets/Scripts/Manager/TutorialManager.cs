using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public enum ETutorialStep { Welcome, MoveExample, AttackExample, ItemExample, AvoidPattern, Clear }
    public ETutorialStep currentStep = ETutorialStep.Welcome;

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

    public void StartTutorial()
    {
        EnterStep(ETutorialStep.MoveExample);
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
            case ETutorialStep.Welcome:
                welcomePanel.SetActive(true);
                break;

            case ETutorialStep.MoveExample:
                moveGuidePanel.SetActive(true);
                // 기획적으로 플레이어가 이동만 할 수 있게 다른 버튼을 비활성화하는 로직 추가 가능
                break;

            case ETutorialStep.AttackExample:
                attackGuidePanel.SetActive(true);
                break;

            case ETutorialStep.Clear:
                EndTutorial();
                break;
        }
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    // 외부(Player나 UI 버튼)에서 무언가 완료했을 때 호출하는 함수
    public void CompleteCurrentStep()
    {
        if (currentStep == ETutorialStep.Welcome)
            EnterStep(ETutorialStep.MoveExample);
        else if (currentStep == ETutorialStep.MoveExample)
            EnterStep(ETutorialStep.AttackExample);
        else if (currentStep == ETutorialStep.AttackExample)
            EnterStep(ETutorialStep.Clear);
    }

    private void EndTutorial()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isTutorialCleared = true;
        }

        // 빌드 세팅에 등록된 메인 보스전 씬으로 전환
        SceneManager.LoadScene("Scene_Valtan");
    }
}