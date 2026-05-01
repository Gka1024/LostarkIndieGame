using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossBuffIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite iconImage;
    public const float DESCTRIPTION_OFFSET_Y = -200;

    [SerializeField] private GameObject bufficonImage;
    [SerializeField] private BossBuffData data;

    private GameObject toolTipUI;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descText;

    [SerializeField] private GameObject stackNum;
    private int stack;

    [SerializeField] private GameObject durationNum;
    private int duration;

    public void Init(BossBuff buff, GameObject toolTip, TextMeshProUGUI name, TextMeshProUGUI desc, int stack, int duration)
    {
        data = buff.Data;
        this.toolTipUI = toolTip;
        this.nameText = name;
        this.descText = desc;
        this.stack = stack;
        this.duration = duration;

        stackNum = transform.GetChild(1).gameObject;
        durationNum = transform.GetChild(2).gameObject;

        ChangeIconImage();

        DisplayStackNumber();
        DisplayDurationNumber();

    }

    private void ChangeIconImage()
    {
        iconImage = data.Icon;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconImage;
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

    private void DisplayDurationNumber()
    {
        if (duration != -1)
        {
            durationNum.GetComponent<TextMeshProUGUI>().SetText(duration + "턴");
            durationNum.SetActive(true);
        }
        else
        {
            durationNum.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameText.text = data.buffName;
        descText.text = data.description;
        MoveToolTipUI();
        toolTipUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipUI.SetActive(false);
    }

    private void MoveToolTipUI()
    {
        toolTipUI.transform.position = this.gameObject.transform.position + new Vector3(0, DESCTRIPTION_OFFSET_Y, 0);
    }
}