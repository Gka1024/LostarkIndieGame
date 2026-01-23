using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public RectTransform currentBossHP; // HP 마스크
    public RectTransform nextBossHP; // 다음 HP 마스크

    public GameObject[] HealthLines; // HP 줄 배열 빨강 -> 보라 -> 파랑 -> 하늘 -> 연두 -> 노랑 -> 주황 
    public GameObject[] BackgroundLines; // HP 줄 배경 배열
    public GameObject LastHP; // 마지막 HP 줄

    [SerializeField] private bool hasBossShield;
    public RectTransform shieldBar;
    public float currentShield;

    public TextMeshProUGUI text;

    public float MaxHealth = 24000; // 보스 총 체력 (160줄 * 150)
    public float currentHealth;

    public float healthPerStage = 150; // 한 줄당 체력
    private float currentHealthOnStage;

    [SerializeField]
    private int currentStage;
    private float maskFullWidth;

    public GameObject defenceDownDebuff;

    private void Start()
    {
        currentHealth = MaxHealth;
        currentHealthOnStage = healthPerStage;
        currentStage = 0;
        maskFullWidth = currentBossHP.sizeDelta.x;

        UpdateHPBar(false);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealthOnStage -= damage;

        bool isBarColorChange = false;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            currentHealthOnStage = 0;
            text.SetText("X0");
            GameOver();
        }

        if (currentHealthOnStage <= 0)
        {
            while (currentHealthOnStage < 0)
            {
                currentStage++; // 체력 줄 이동
                currentHealthOnStage += healthPerStage; // 다음 체력 줄 채우기
                isBarColorChange = true;
            }
        }

        UpdateHPBar(isBarColorChange);
    }

    private void ResetHPBar()
    {
        for (int i = 0; i < HealthLines.Length; i++)
        {
            HealthLines[i].SetActive(false);
            BackgroundLines[i].SetActive(false);
        }
    }

    private void UpdateHPBar(bool isBarColorChange)
    {
        float bossHPRatio = Mathf.Clamp01(currentHealthOnStage / healthPerStage); // 비율 계산 (0~1 사이로 제한)
        currentBossHP.sizeDelta = new Vector2(maskFullWidth * bossHPRatio, currentBossHP.sizeDelta.y);

        if (isBarColorChange)
        {
            ResetHPBar();
            text.SetText("X" + (160 - currentStage).ToString()); // 남은 체력 줄 표시
            HealthLines[currentStage % 7].SetActive(true); // 현재 스테이지 HP 줄 활성화

            if (currentStage != 159)
            {
                BackgroundLines[(currentStage + 1) % 7].SetActive(true); // 현재 스테이지 HP 줄 배경 활성화
            }
            else
            {
                LastHP.SetActive(true); // 마지막 HP 줄 활성화
            }
        }
    }

    public void UpdateShieldBar(float shield)
    {
        currentShield = shield;

        if (shield <= 0)
        {
            shieldBar.gameObject.SetActive(false);
            return;
        }

        shieldBar.gameObject.SetActive(true);

        // 보호막은 현재 체력줄 기준으로 표시
        float shieldRatio = Mathf.Clamp01(shield / healthPerStage);
        shieldBar.sizeDelta = new Vector2(maskFullWidth * shieldRatio, shieldBar.sizeDelta.y);
    }

    public void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, MaxHealth); // 0 ~ MaxHealth 사이로 제한
        currentStage = (int)((MaxHealth - currentHealth) / healthPerStage); // 현재 줄 계산
        currentHealthOnStage = healthPerStage - ((MaxHealth - currentHealth) % healthPerStage); // 해당 줄의 체력

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            currentHealthOnStage = 0;
            text.SetText("X0");
            GameOver();
        }
        else
        {
            text.SetText("X" + (160 - currentStage).ToString());
        }

        ResetHPBar();
        HealthLines[currentStage % 7].SetActive(true);

        if (currentStage != 159)
        {
            BackgroundLines[(currentStage + 1) % 7].SetActive(true);
        }
        else
        {
            LastHP.SetActive(true);
        }

        float bossHPRatio = Mathf.Clamp01(currentHealthOnStage / healthPerStage);
        currentBossHP.sizeDelta = new Vector2(maskFullWidth * bossHPRatio, currentBossHP.sizeDelta.y);
    }

    public void ShowDebuff(bool show)
    {
        defenceDownDebuff.SetActive(show);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}
