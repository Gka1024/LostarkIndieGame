using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameManager manager;
    public HexTileManager tileManager;
    public HexTileSelectHandler hexTileSelectHandler;
    public ObjectClickHandler objectClickHandler;
    public SkillManager skillManager;

    [SerializeField]
    private Vector3 centerPosition = new Vector3(350, -450, 0); // 카드들의 중앙 위치
    public float cardSpace; // 카드 간격 

    public Transform cardParent; // 카드들이 위치할 부모 오브젝트 (Canvas 내부)

    public GameObject currentCard; // 현재 사용한 카드
    public List<GameObject> cardsInHand = new List<GameObject>(); // 현재 손에 있는 카드 목록
    public List<GameObject> disposeableCards = new(); // 삭제할 카드를 담아두는 배열
    public CardList cardList; // 사용할 수 있는 카드를 저장해두는 게임오브젝트

    private const int MAX_CARD_RETRY = 20;
    private HashSet<int> cardsInHandID = new();

    public GameObject cardDispose; // 카드 삭제 버튼
    public GameObject TripodButton; // 트라이포드를 띄울 때 사용하는 UI
    public GameObject TripodCancelButton; // 트라이포드 취소 버튼 

    public void Awake()
    {
        GameObject[] foundCards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in foundCards)
        {
            cardsInHand.Add(card);
        }
        ArrangeCards();
    }

    // ======================
    // 카드 생성 부분
    // ======================

    public void SpawnCard()
    {
        GameObject ranCard = cardList.GetRandomCard();
        GameObject newCard = Instantiate(ranCard, centerPosition, Quaternion.identity, cardParent);

        AddCardsInHand(newCard);
        newCard.GetComponent<CardInteraction>().cardManager = this.gameObject.GetComponent<CardManager>();
        newCard.GetComponent<CardInteraction>().CardDispose = cardDispose;

        // 카드 위치 재조정
        ArrangeCards();
        CheckCardSpace();
    }

    public void SpawnCard(GameObject card)
    {
        GameObject newCard = Instantiate(card, centerPosition, Quaternion.identity, cardParent);

        AddCardsInHand(newCard);
        newCard.GetComponent<CardInteraction>().cardManager = this.gameObject.GetComponent<CardManager>();
        newCard.GetComponent<CardInteraction>().CardDispose = cardDispose;

        // 카드 위치 재조정
        ArrangeCards();
        CheckCardSpace();
    }

    public void SpawnCardByID(int cardId)
    {
        // CardList에서 ID에 해당하는 카드 프리팹을 가져옵니다.
        GameObject cardToSpawn = cardList.GetCardByID(cardId);
        if (cardToSpawn == null)
        {
            Debug.LogWarning($"ID: {cardId}에 해당하는 카드를 찾을 수 없습니다.");
            return;
        }

        GameObject newCard = Instantiate(cardToSpawn, centerPosition, Quaternion.identity, cardParent);

        AddCardsInHand(newCard);
        newCard.GetComponent<CardInteraction>().cardManager = this.gameObject.GetComponent<CardManager>();
        newCard.GetComponent<CardInteraction>().CardDispose = cardDispose;

        // 카드 위치 재조정
        ArrangeCards();
        CheckCardSpace();
    }

    private GameObject GetDistinctCard()
    {
        for (int i = 0; i < MAX_CARD_RETRY; i++)
        {
            GameObject ranCard = cardList.GetRandomCard();
            int cardId = ranCard.GetComponent<CardSkill>().CardID;

            if (!cardsInHandID.Contains(cardId))
            {
                return ranCard;
            }

            if (i == MAX_CARD_RETRY - 1)
            {
                Debug.LogWarning("카드 드랍 불가");
            }
        }

        return null;
    }

    public void GiveCard(int? count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnCard(GetDistinctCard());
        }
    }

    public void GiveSpecificCard(int cardNum)
    {
        SpawnCardByID(cardNum);
    }

    // ======================
    // 손패 조절
    // ======================

    private void AddCardsInHand(GameObject newCard)
    {
        cardsInHand.Add(newCard);
        cardsInHandID.Add(newCard.GetComponent<CardSkill>().CardID);
    }

    public void ResetHand()
    {
        cardsInHandID.Clear();
    }


    // ======================
    // 카드 인터랙션 부분
    // ======================

    public void ArrangeCards()
    {
        int cardCount = cardsInHand.Count;
        float spacing = cardSpace; // 카드 간격

        for (int i = 0; i < cardCount; i++)
        {
            float offsetX = (i - (cardCount - 1) / 2f) * spacing; // 중앙 정렬 계산
            Vector3 newPosition = centerPosition + new Vector3(offsetX, 0, 0);

            RectTransform rectTransform = cardsInHand[i].GetComponent<RectTransform>();
            cardsInHand[i].GetComponent<CardInteraction>().SetDefaultPos(newPosition);
            rectTransform.SetSiblingIndex(i); // 오른쪽 카드가 더 위로 배치되도록 설정
        }
    }

    public void GiveBasicCard()
    {
        SpawnCard(cardList.GetBasicCard());
    }

    private void SetCardSpace(float space)
    {
        cardSpace = space;
        ArrangeCards();
    }

    private void CheckCardSpace()
    {
        switch (cardsInHand.Count)
        {
            case 1:
            case 2:
            case 3:
                SetCardSpace(220);
                break;

            case 4:
            case 5:
            case 6:
                SetCardSpace(180);
                break;

            case 7:
            case 8:
                SetCardSpace(155);
                break;
        }
    }

    public void PointerEnter(CardInteraction card)
    {
        int cardIndex = cardsInHand.IndexOf(card.gameObject);
        CancelPlayerClicked();
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (i < cardIndex)
            {
                // 왼쪽에 있는 카드
                cardsInHand[i].GetComponent<CardInteraction>().SetDefaultPos(cardsInHand[i].GetComponent<CardInteraction>().GetDefaultPos() - new Vector3(50, 0, 0));
            }
            else if (i > cardIndex)
            {
                // 오른쪽에 있는 카드
                cardsInHand[i].GetComponent<CardInteraction>().SetDefaultPos(cardsInHand[i].GetComponent<CardInteraction>().GetDefaultPos() + new Vector3(50, 0, 0));
            }
        }
    }

    public void PointerExit(CardInteraction card)
    {
        int cardIndex = cardsInHand.IndexOf(card.gameObject);

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (i < cardIndex)
            {
                // 왼쪽에 있는 카드
                cardsInHand[i].GetComponent<CardInteraction>().SetDefaultPos(cardsInHand[i].GetComponent<CardInteraction>().GetDefaultPos() + new Vector3(50, 0, 0));
            }
            else if (i > cardIndex)
            {
                // 오른쪽에 있는 카드
                cardsInHand[i].GetComponent<CardInteraction>().SetDefaultPos(cardsInHand[i].GetComponent<CardInteraction>().GetDefaultPos() - new Vector3(50, 0, 0));
            }
        }
    }

    // ======================
    // 카드 사용 부분
    // ======================

    public void UseCard(GameObject obj)
    {
        currentCard = obj;
        currentCard.GetComponent<CardText>().SetTripodText();
        skillManager.StartSkillSequence(currentCard);
    }

    // ======================
    // 카드 삭제 부분
    // ======================

    public void ClearCard()
    {
        foreach (GameObject obj in disposeableCards)
        {
            Destroy(obj);
        }
        disposeableCards.Clear();
    }

    public void PutToDisposeable(GameObject obj)
    {
        disposeableCards.Add(obj);
    }

    public void DisposeCard(GameObject card)
    {
        if (cardsInHand.Contains(card))
        {
            cardsInHand.Remove(card);  // 리스트에서 먼저 제거
        }
        card.GetComponent<CardInteraction>().DisposeCard();

        PutToDisposeable(card);

        ArrangeCards();  // 삭제 후 재정렬
        CheckCardSpace(); // 간격도 다시 계산
    }

    public void DisposeAllCards()
    {
        List<GameObject> cardsToRemove = new(cardsInHand);
        foreach (GameObject obj in cardsToRemove)
        {
            DisposeCard(obj);
        }

        ClearCard();
    }

    // ======================
    // 기타 유틸
    // ======================

    private void CancelPlayerClicked()
    {
        objectClickHandler.isPlayerClicked = false;
        HexTileManager.Instance.ResetTileColor();
    }

    public void ShowCards(bool show)
    {
        foreach (GameObject card in cardsInHand)
        {
            if (card == null) continue; // 오브젝트가 파괴된 경우 무시

            CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                StartCoroutine(SetCardVisible(canvasGroup, show));
            }
        }
    }

    private IEnumerator SetCardVisible(CanvasGroup canvasGroup, bool show)
    {
        float duration = 0.2f;
        float time = 0f;

        int from = show ? 0 : 1;
        int to = show ? 1 : 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
        canvasGroup.blocksRaycasts = show;
    }

    public void OnTurnEnd()
    {
        cardList.OnTurnEnd();
    }
}