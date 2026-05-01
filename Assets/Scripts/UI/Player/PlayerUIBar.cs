using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUIBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject text;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI identityText;

    public PlayerStatsSpecific curStats;

    private void GetTextFromStats()
    {
        PlayerStats stat = Player.Instance.stats;

        if (stat.HasPlayerShield())
        {
            healthText.SetText($"({stat.buffState.GetCurrentShield()}) {stat.currentHealth} / {PlayerStats.MAX_HEALTH}");
        }
        else
        {
            healthText.SetText($"{stat.currentHealth} / {PlayerStats.MAX_HEALTH}");
        }

        manaText.SetText($"{stat.currentMana} / {PlayerStats.MAX_MANA}");
        identityText.SetText($"{stat.currentIdentity} / {PlayerStats.MAX_IDENTITY}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetTextFromStats();
        text.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.SetActive(false);
    }
}

public enum PlayerStatsSpecific
{
    health,
    mana,
    identity
}
