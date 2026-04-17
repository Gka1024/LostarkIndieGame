using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BossBuffsUI : MonoBehaviour
{
    public BossStatus bossStatus;

    public GameObject buffsParentGameObject;
    public GameObject debuffsParentGameObject;

    public Transform buffIconStartTransform;
    public Transform debuffIconStartTransform;
    private const float ICON_MOVE_MARGIN = 60;

    public GameObject bossBuffIcon; // prefab
    public GameObject buffDescribtionUI;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;

    public Dictionary<int, BossBuff> bossBuffsCopy;
    public Dictionary<int, BossBuff> bossDebuffsCopy;

    public void OnTurnStart()
    {
        bossStatus.AlertBuffsUpdate();
    }

    public void UpdateBuffs(Dictionary<int, BossBuff> buffs, Dictionary<int, BossBuff> debuffs)
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
        foreach (Transform child in buffsParentGameObject.transform)
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
                buffsParentGameObject.transform);

            BuffIconUI iconUI = iconObj.GetComponent<BuffIconUI>();

            iconUI.Init(buff, buffDescribtionUI, buffName, buffDesc, buff.Stack, buff.Duration);

            if (buff.Data == null)
            {
                Debug.LogError($"Buff [{buff.ID}] 의 data가 null입니다!");
            }

            index++;
        }

    }

    private void UpdateDebuffUI()
    {
        foreach (Transform child in debuffsParentGameObject.transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var kvp in bossDebuffsCopy)
        {
            var buff = kvp.Value;

            Vector3 move = new Vector3(ICON_MOVE_MARGIN * index, 0, 0);
            var iconObj = Instantiate(bossBuffIcon,
                debuffIconStartTransform.position + move,
                debuffIconStartTransform.rotation,
                debuffsParentGameObject.transform);

            BuffIconUI iconUI = iconObj.GetComponent<BuffIconUI>();

            iconUI.Init(buff, buffDescribtionUI, buffName, buffDesc, buff.Stack,buff.Duration);

            if (buff.Data == null)
            {
                Debug.LogError($"Buff [{buff.ID}] 의 data가 null입니다!");
            }

            index++;
        }
    }

}
