using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite iconImage;

    [SerializeField] private GameObject bufficonImage;
    [SerializeField] private BossBuffData data;

    private GameObject toolTipUI;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descText;

    [SerializeField] private GameObject stackNum;
    private int stack;

    public void Init(BossBuff buff, GameObject toolTip, TextMeshProUGUI name, TextMeshProUGUI desc, int stack)
    {
        data = buff.data;
        this.toolTipUI = toolTip;
        this.nameText = name;
        this.descText = desc;
        this.stack = stack;
        iconImage = data.Icon;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconImage;
        stackNum = transform.GetChild(1).gameObject;
        DisplayStackNumber();
    }

    private void DisplayStackNumber()
    {
        if (stack >= 2)
        {
            stackNum.GetComponent<TextMeshProUGUI>().SetText("X " + stack);
            stackNum.SetActive(true);
        }
        else
        {
            stackNum.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameText.text = data.buffName;
        descText.text = data.description;

        toolTipUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipUI.SetActive(false);
    }
}