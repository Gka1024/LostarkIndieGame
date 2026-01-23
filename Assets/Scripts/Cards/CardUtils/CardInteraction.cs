using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardManager cardManager;
    public GameObject CardHighlightBackground;
    public GameObject CardDispose;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffset;

    [SerializeField]
    private Vector3 defaultCardPos;
    public int CardUpThreshold = 250; // 카드 사용을 위한 드래그 거리
    public float lerpSpeed = 30f; // Lerp 속도
    public float cardParabolaVariable = 80f; // 카드가 Dispose 위치로 이동할 때 포물선의 높이

    private Vector3 disposeStartPosition;
    private Vector3 disposeEndPosition;
    private bool isDisposing = false;
    private float disposeTime = 0f;
    public float disposeDuration = 0.2f; // 카드가 Dispose 위치로 이동하는 데 걸리는 시간
    public float hoverOffset = 10f;

    [HideInInspector]
    public static bool isDragging = false;

    private void Awake()
    {
        cardManager = FindAnyObjectByType<CardManager>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        CardDispose = GameObject.FindGameObjectWithTag("CardDispose");
        SetDefaultPos();
    }

    private void SetDefaultPos()
    {
        defaultCardPos = rectTransform.anchoredPosition;
    }

    public Vector3 GetDefaultPos()
    {
        return defaultCardPos;
    }

    public void SetDefaultPos(Vector3 pos)
    {
        defaultCardPos = pos;
    }

    public void Update()
    {
        if (!isDisposing)
        {
            CardMove();
            CardHighlight();
        }
    }

    private void CardMove()
    {
        if (!isDragging)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, defaultCardPos, Time.deltaTime * lerpSpeed);
        }
    }

    private void CardHighlight()
    {
        CardHighlightBackground.SetActive(IsCardUpEnough());
    }

    private bool IsCardUpEnough()
    {
        return rectTransform.anchoredPosition.y > defaultCardPos.y + CardUpThreshold;
    }

    public void DisposeCard()
    {
        disposeStartPosition = rectTransform.anchoredPosition;
        disposeEndPosition = CardDispose.GetComponent<RectTransform>().anchoredPosition;
        isDisposing = true;
        disposeTime = 0f;
        StartCoroutine(DisposeCardAnimation());
    }

    private IEnumerator DisposeCardAnimation()
    {
        while (disposeTime < 1f)
        {
            disposeTime += Time.deltaTime / disposeDuration;
            float height = Mathf.Sin(Mathf.PI * disposeTime) * cardParabolaVariable;
            Vector3 currentPosition = Vector3.Lerp(disposeStartPosition, disposeEndPosition, disposeTime);
            currentPosition.y += height;
            rectTransform.anchoredPosition = currentPosition;
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, disposeTime);
            yield return null;
        }

        isDisposing = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragOffset = eventData.position - (Vector2)rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        ))
        {
            rectTransform.anchoredPosition = localPoint - dragOffset; // 마우스 위치 보정하여 이동
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (IsCardUpEnough())
        {
            UseCard();
        }
    }

    private void UseCard()
    {
        Debug.Log("Card Used: " + gameObject.name);
        cardManager.GetComponent<CardManager>().UseCard(this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;
        rectTransform.SetAsLastSibling();
        defaultCardPos += new Vector3(0, hoverOffset, 0);
        cardManager.GetComponent<CardManager>().PointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;
        defaultCardPos -= new Vector3(0, hoverOffset, 0);
        cardManager.GetComponent<CardManager>().PointerExit(this);
        cardManager.GetComponent<CardManager>().ArrangeCards();
    }
}