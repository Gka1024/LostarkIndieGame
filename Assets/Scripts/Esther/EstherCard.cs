using UnityEngine;
using UnityEngine.EventSystems;

public class EstherCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public EstherManager estherManager;

    private RectTransform rectTransform;
    public float hoverOffset;
    private float lerpSpeed = 30f;
    [SerializeField] private Vector3 defaultCardPos;

    public GameObject backgroundImage;

    public EstherType estherType;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SetDefaultPos();
    }

    public void Update()
    {
        CardMove();
    }

    private void CardMove()
    {
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, defaultCardPos, Time.deltaTime * lerpSpeed);
    }

    private void SetDefaultPos()
    {
        defaultCardPos = rectTransform.anchoredPosition;
    }

    public void ShowBackground(bool show)
    {
        backgroundImage.SetActive(show);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        defaultCardPos += new Vector3(0, hoverOffset, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        defaultCardPos -= new Vector3(0, hoverOffset, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseEstherSkill();
    }

    private void UseEstherSkill()
    {
        estherManager.UseEstherSkill(estherType);
    }
}
