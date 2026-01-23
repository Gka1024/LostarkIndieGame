using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public CardManager cardManager;
    public static CardList Instance { get; private set; }

    [Header("전체 카드 및 기본 카드 목록")]
    public GameObject basicCard;

    public List<GameObject> cardsList = new();
    public List<CardStats> cardStatsList = new();

    public List<GameObject> chainCardsList = new();

    [Header("카드 ID 관리용")]
    private Dictionary<int, CardStats> cardStatsDictionary = new();
    private Dictionary<int, GameObject> cardsDictionary = new();
    private Dictionary<(int, int), GameObject> chainsDictionary = new();
    private Dictionary<int, int> cardsCooldown = new(); // CardID , 남은 쿨타임
    private HashSet<int> cardsInHands = new(); // 현재 손에 있는 카드 ID들

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }
        RegisterCardStats();
        RegisterCardID();
        RegisterChainID();
    }

    // ==== 카드 등록 관련 함수

    private void RegisterCardStats()
    {
        foreach (var stat in cardStatsList)
        {
            cardStatsDictionary.Add(stat.CardID, stat);
        }
    }

    private void RegisterCardID()
    {
        foreach (var card in cardsList)
        {
            cardsDictionary.Add(card.GetComponent<CardSkill>().CardID, card);
        }
    }

    private void RegisterChainID()
    {
        foreach (GameObject obj in chainCardsList)
        {
            ChainSkill skill = obj.GetComponent<ChainSkill>();
            chainsDictionary.Add((skill.CardID, skill.CardTripodNum), obj);
        }
    }

    // ===== 카드 및 카드 속성을 리턴하는 함수

    public GameObject GetCardByID(int cardID)
    {
        if (cardsDictionary.TryGetValue(cardID, out GameObject card))
        {
            return card;
        }
        Debug.LogWarning($"Card with ID {cardID} not found!");
        return null;
    }

    public CardStats GetCardStats(int index)
    {
        cardStatsDictionary.TryGetValue(index, out CardStats stats);
        return Instantiate(stats);
    }

    public GameObject GetChainSkills(int cardID, int tripodNum)
    {
        chainsDictionary.TryGetValue((cardID, tripodNum), out GameObject obj);
        return obj;
    }

    // ===== 랜덤 카드 관련 함수

    // 랜덤 카드 1장 반환
    public GameObject GetRandomCard()
    {
        List<GameObject> candidates = GetAvailableCards();

        if (candidates.Count == 0)
        {
            Debug.Log("사용 가능한 카드가 없습니다.");
            return null;
        }

        GameObject selectedCard = candidates[Random.Range(0, candidates.Count)];
        var stats = selectedCard.GetComponent<CardSkill>().runtimeCardStats;
        if (stats != null)
        {
            cardsInHands.Add(stats.CardID);
        }

        return selectedCard;
    }

    // 기본 카드 전체 반환
    public GameObject GetBasicCard()
    {
        return basicCard;
    }

    // 손에서 제거된 카드 ID 제거
    public void RemoveCardFromHand(CardStats stats)
    {
        if (stats != null)
        {
            cardsInHands.Remove(stats.CardID);
        }
    }

    // 현재 사용 가능한 카드 목록 반환
    private List<GameObject> GetAvailableCards()
    {
        List<GameObject> result = new();

        foreach (var kvp in cardsDictionary)
        {
            int cardID = kvp.Key;
            GameObject cardObj = kvp.Value;

            bool isOnCooldown = cardsCooldown.ContainsKey(cardID) && cardsCooldown[cardID] > 0;
            bool isInHand = cardsInHands.Contains(cardID);

            if (!isOnCooldown && !isInHand)
            {
                result.Add(cardObj);
            }
        }

        return result;
    }

    // ===== 쿨타임 관련 함수 

    // 카드 사용 시 쿨다운 설정
    public void ApplyCardCooldown(CardStats stats)
    {
        if (stats != null && stats.coolDownTurn > 0)
        {
            int cardID = stats.CardID;
            cardsCooldown[cardID] = stats.coolDownTurn;
        }
    }

    // 모든 카드의 쿨타임 1씩 감소
    public void ProgressCooldownTurn()
    {
        List<int> keys = new(cardsCooldown.Keys);

        foreach (int key in keys)
        {
            if (cardsCooldown[key] > 0)
            {
                cardsCooldown[key]--;
            }
        }
    }

    // 카드의 쿨타임 초기화
    public void ResetCooldownByID(int targetID)
    {
        if (cardsCooldown.ContainsKey(targetID))
        {
            cardsCooldown[targetID] = 0;
            Debug.Log($"Card ID {targetID}의 쿨타임이 초기화되었습니다.");
        }
        else
        {
            Debug.LogWarning($"Card ID {targetID}를 찾을 수 없습니다.");
        }
    }

    public void ResetQuickMove()
    {
        ResetCooldownByID(2);
        cardManager.SpawnCard(GetCardByID(2));
    }

    // 턴 종료시 실행
    public void OnTurnEnd()
    {
        cardsInHands.Clear();
        ProgressCooldownTurn();
    }
    // =====









}
