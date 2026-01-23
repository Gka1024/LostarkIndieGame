using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase Instance { get; private set; }

    public TextAsset jsonFile;
    public List<CardData> cardList = new();

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            LoadCardData();
        }
        else
        {
            Destroy(gameObject); // 이미 있으면 새로 생긴 건 파괴
        }
    }

    public void LoadCardData()
    {
        if (jsonFile != null)
        {
            CardDataList cardDataList = JsonUtility.FromJson<CardDataList>(jsonFile.text);
            cardList = cardDataList.cards;
        }
        else
        {
            Debug.LogError("json 파일을 찾을 수 없습니다.");
        }
    }

    public CardData GetCardById(int id)
    {
        return cardList.Find(card => card.id == id);
    }

    public string GetCardName(int id)
    {
        CardData card = GetCardById(id);
        return card != null ? card.name : null;
    }
}
