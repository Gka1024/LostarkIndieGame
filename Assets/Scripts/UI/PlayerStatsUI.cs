using System;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats playerStats;

    private const float HP_BAR_MAX_X = 420;
    private float PlayerMaxHP;
    public RectTransform PlayerHPBar;

    private const float SHIELD_BAR_MAX_X = 420;
    public RectTransform PlayerShieldBar;

    private const float MANA_BAR_MAX_X = 420;
    private float PlayerMaxMana;
    public RectTransform PlayerManaBar;

    public GameObject playerBuffIcon;


    public void Awake()
    {
        PlayerMaxHP = playerStats.maxHealth;
        PlayerMaxMana = playerStats.maxMana;
    }

    public void UpdateHPBar(float currentHealth)
    {
        float healthRatio = Mathf.Clamp01(currentHealth / PlayerMaxHP);
        PlayerHPBar.sizeDelta = new Vector2(healthRatio * HP_BAR_MAX_X, PlayerHPBar.sizeDelta.y);
    }

    public void UpdateShieldBar(float currentShield)
    {
        float shieldRatio = Mathf.Clamp01(currentShield / PlayerMaxHP);
        PlayerShieldBar.sizeDelta = new Vector2(shieldRatio * SHIELD_BAR_MAX_X, PlayerShieldBar.sizeDelta.y);
    }

    public void UpdateManaBar(float currentMana)
    {
        float manaRatio = Mathf.Clamp01(currentMana / PlayerMaxMana);
        PlayerManaBar.sizeDelta = new Vector2(manaRatio * MANA_BAR_MAX_X, PlayerManaBar.sizeDelta.y);
    }

    public void ShowPlayerBuffIcon(bool show)
    {
        playerBuffIcon.SetActive(show);
    }
}
