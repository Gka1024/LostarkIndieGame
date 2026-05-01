using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBuffIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite iconImage;
    public const float DESCTRIPTION_OFFSET_Y = -150;

    [SerializeField] private GameObject bufficonImage;
    [SerializeField] private PlayerBuffData data;

    private GameObject toolTipUI;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descText;
    public TextMeshProUGUI durationText;

    [SerializeField] private GameObject stackNum;
    private int stack;

    [SerializeField] private GameObject durationNum;
    private int duration;

    [SerializeField] private Image image;

    public void Init(PlayerBuff buff, GameObject toolTip, TextMeshProUGUI name, TextMeshProUGUI desc, int stack, int duration)
    {
        data = buff.Data;
        this.toolTipUI = toolTip;
        this.nameText = name;
        this.descText = desc;
        this.stack = stack;
        this.duration = duration;


        ChangeIconImage();

    }

    private void ChangeIconImage()
    {
        iconImage = data.Icon;
        if (iconImage != null) image.sprite = iconImage;
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