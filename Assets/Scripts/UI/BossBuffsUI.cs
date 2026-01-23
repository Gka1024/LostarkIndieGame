using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BossBuffsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BossStatus bossStatus;

    public GameObject buffs;
    public GameObject debuffs;

    public Transform buffIconStartTransform;
    public Transform debuffIconStartTransform;
    private const float ICON_MOVE_MARGIN = 150;

    public GameObject bossBuffIcon; // prefab
    public GameObject buffDescribtionUI;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;

    public Dictionary<int, BossBuff> bossBuffsCopy;
    public Dictionary<int, BossDebuff> bossDebuffsCopy;

    public void OnTurnStart()
    {
        bossBuffsCopy = bossStatus.GetBossBuffs();
    }

    public void UpdateBuffs(Dictionary<int, BossBuff> buffs, Dictionary<int, BossDebuff> debuffs)
    {
        this.bossBuffsCopy = buffs;
        this.bossDebuffsCopy = debuffs;
        UpdateBuffUI();
        UpdateDebuffUI();
    }

    public void Callupdatebuffui()
    {
        UpdateBuffUI();
    }

    private void UpdateBuffUI()
    {
        foreach (Transform child in buffs.transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var kvp in bossBuffsCopy)
        {
            var buff = kvp.Value;

            Vector3 move = new Vector3(ICON_MOVE_MARGIN * index, 0, 0);
            var iconObj = Instantiate(bossBuffIcon,
                buffIconStartTransform.position + move,
                buffIconStartTransform.rotation,
                buffs.transform);

            BuffIconUI iconUI = iconObj.GetComponent<BuffIconUI>();

           iconUI.Init(buff, buffDescribtionUI, buffName, buffDesc, buff.stack);

            if (buff.data == null)
            {
                Debug.LogError($"Buff [{buff.buffID}] 의 data가 null입니다!");
            }

            index++;
        }

    }

    private void UpdateDebuffUI()
    {
        foreach (Transform child in debuffs.transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var buff in bossDebuffsCopy)
        {
            Vector3 MoveVector = new Vector3(ICON_MOVE_MARGIN * index, 0, 0);
            Instantiate(bossBuffIcon, debuffIconStartTransform.position + MoveVector, debuffIconStartTransform.rotation, buffs.transform);
            index++;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        buffDescribtionUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffDescribtionUI.SetActive(false);
    }


}
