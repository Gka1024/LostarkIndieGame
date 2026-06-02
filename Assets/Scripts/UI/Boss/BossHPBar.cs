using TMPro;
using UnityEngine;

public class BossHPBar : MonoBehaviour
{
    [Header("HP Bar UI")]
    public RectTransform currentBossHP;  // HP 마스크
    public RectTransform nextBossHP;     // 다음 HP 마스크 (필요 시 활용)
    public TextMeshProUGUI textBossHP;
    public TextMeshProUGUI textBossHPLine;

    [Header("HP Lines")]
    public GameObject[] HealthLines;     // 빨강 -> 보라 -> 파랑 -> 하늘 -> 연두 -> 노랑 -> 주황 
    public GameObject[] BackgroundLines; // HP 줄 배경 배열
    public GameObject LastHP;            // 마지막 HP 줄

    [Header("Shield")]
    public RectTransform shieldBar;
    public float currentShield;

    [Header("Debuff & Settings")]
    public GameObject defenceDownDebuff;
    public float healthPerStage = 150f;  // 한 줄당 체력

    // 캡슐화를 통해 외부에서 직접 수정하지 못하도록 프로퍼티/private 설정
    public float currentHealth { get; private set; }
    private float MaxHealth;
    private int MaxHealthLine;
    private float maskFullWidth;

    private void Start()
    {
        // 1. 초기 데이터 설정
        MaxHealth = GameManager.Instance.isTutorialCleared ? BossStats.MAX_HEALTH : TutorialBossStats.MAX_HEALTH_TUTORIAL;
        currentHealth = MaxHealth;
        MaxHealthLine = Mathf.CeilToInt(MaxHealth / healthPerStage);
        Canvas.ForceUpdateCanvases();
        maskFullWidth = 839f;

        // 2. 초기 UI 상태 동기화
        RefreshUI();
    }

    /// <summary>
    /// 보스에게 데미지를 입힙니다.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        RefreshUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 외부에서 보스의 체력을 강제로 설정할 때 사용합니다.
    /// </summary>
    public void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, MaxHealth);

        RefreshUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 현재 체력(currentHealth)을 기반으로 모든 UI를 한 번에 새로고침합니다.
    /// </summary>
    private void RefreshUI()
    {
        // 1. 텍스트 가독성 최적화
        textBossHP.SetText($"{(int)currentHealth} / {MaxHealth}");

        if (currentHealth <= 0)
        {
            textBossHPLine.SetText("X0");
            currentBossHP.sizeDelta = new Vector2(0, currentBossHP.sizeDelta.y);
            ResetHPBar();
            return;
        }

        // 2. 현재 체력이 위치한 '줄(Stage)'과 '해당 줄에 남은 체력' 계산
        float consumedHealth = MaxHealth - currentHealth;
        int currentStage = (int)(consumedHealth / healthPerStage);

        // [수정] 체력이 가득 차 있을 때(소모량이 0일 때) 나머지가 0이 되어 게이지가 사라지는 현상 방지
        float currentHealthOnStage;
        if (currentHealth >= MaxHealth)
        {
            currentStage = 0;
            currentHealthOnStage = healthPerStage;
        }
        else
        {
            currentHealthOnStage = healthPerStage - (consumedHealth % healthPerStage);
        }

        // 3. 체력 바 게이지 조절 (비율 계산)
        float bossHPRatio = Mathf.Clamp01(currentHealthOnStage / healthPerStage);
        currentBossHP.sizeDelta = new Vector2(maskFullWidth * bossHPRatio, currentBossHP.sizeDelta.y);

        // 4. 남은 체력 줄 개수 텍스트 표기
        int remainingLines = MaxHealthLine - currentStage;
        textBossHPLine.SetText($"X{remainingLines}");

        // 5. HP 줄 및 배경 오브젝트 제어
        ResetHPBar();

        int currentLineIndex = currentStage % 7;
        HealthLines[currentLineIndex].SetActive(true);

        if (currentStage < MaxHealthLine - 1)
        {
            int nextLineIndex = (currentStage + 1) % 7;
            BackgroundLines[nextLineIndex].SetActive(true);
        }
        else
        {
            if (LastHP != null) LastHP.SetActive(true);
        }
    }

    /// <summary>
    /// 보호막 바 UI를 업데이트합니다.
    /// </summary>
    public void UpdateShieldBar(float shield)
    {
        currentShield = shield;

        if (shield <= 0)
        {
            shieldBar.gameObject.SetActive(false);
            return;
        }

        shieldBar.gameObject.SetActive(true);
        float shieldRatio = Mathf.Clamp01(shield / healthPerStage);
        shieldBar.sizeDelta = new Vector2(maskFullWidth * shieldRatio, shieldBar.sizeDelta.y);
    }

    private void ResetHPBar()
    {
        for (int i = 0; i < HealthLines.Length; i++)
        {
            if (HealthLines[i] != null) HealthLines[i].SetActive(false);
            if (BackgroundLines[i] != null) BackgroundLines[i].SetActive(false);
        }
        if (LastHP != null) LastHP.SetActive(false);
    }

    public void ShowDebuff(bool show) => defenceDownDebuff.SetActive(show);

    public void GameOver() => Debug.Log("Game Over");
}