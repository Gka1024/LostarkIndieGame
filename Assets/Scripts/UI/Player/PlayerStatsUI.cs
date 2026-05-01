using System;
using System.Collections.Generic;
using TMPro;
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

    private float PlayerMaxIdentity;

    public RectTransform identityMask;
    private float maskFullHeight;
    public GameObject identityBackGround;

    private const float ICON_MOVE_MARGIN = 65;

    public GameObject playerBuffIcon;

    public Transform buffIconStartTransform;
    public GameObject BuffToolTipUI;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;

    public GameObject buffsParentGameObject;
    public GameObject debuffsParentGameObject;


    public List<PlayerBuff> PlayerBuffsCopy;
    public List<PlayerBuff> PlayerDebuffsCopy;


    public void Awake()
    {
        PlayerMaxHP = PlayerStats.MAX_HEALTH;
        PlayerMaxMana = PlayerStats.MAX_MANA;
        PlayerMaxIdentity = PlayerStats.MAX_IDENTITY;
        
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

    public void UpdateIdentityBar(float currentIdentity)
    {
        float identityRatio = Mathf.Clamp01(currentIdentity / PlayerMaxIdentity);
        identityMask.sizeDelta = new Vector2(identityMask.sizeDelta.x, maskFullHeight * identityRatio);

        if(currentIdentity == PlayerMaxIdentity) SetIdentityReady(true);
    }

    public void SetIdentityReady(bool show)
    {
        identityBackGround.SetActive(show);
    }

    public void UpdateBuffs(List<PlayerBuff> buffs)
    {
        this.PlayerBuffsCopy = buffs;
        //this.PlayerDebuffsCopy = debuffs;
        UpdateBuffUI();
        //UpdateDebuffUI();
    }

    private void UpdateBuffUI()
    {
        foreach (Transform child in buffsParentGameObject.transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (PlayerBuff buff in PlayerBuffsCopy)
        {
            Vector3 move = new Vector3(ICON_MOVE_MARGIN * index, 0, 0);
            var iconObj = Instantiate(playerBuffIcon,
                buffIconStartTransform.position + move,
                buffIconStartTransform.rotation,
                buffsParentGameObject.transform);

            PlayerBuffIconUI iconUI = iconObj.GetComponent<PlayerBuffIconUI>();

            iconUI.Init(buff, BuffToolTipUI, buffName, buffDesc, buff.Stack, buff.Duration);

            if (buff.Data == null)
            {
                Debug.LogError($"Buff [{buff.ID}] 의 data가 null입니다!");
            }
            index++;
        }

    }

    public void ShowPlayerBuffIcon(bool show)
    {
        playerBuffIcon.SetActive(show);
    }
}
