using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CardText : MonoBehaviour
{
    public CardDataBase cardDataBase;
    public UIManager uiManager;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDes;

    public bool isCardDataNull = false;

    [SerializeField] protected CardData cardData;
    [SerializeField] protected CardStats cardStats;
    protected PlayerStats playerStats;

    protected virtual void Start()
    {
        //OnStart();
        StartCoroutine(DelayedStart());
    }

    private void OnStart()
    {
        cardStats = CardList.Instance.GetCardStats(GetComponent<CardSkill>().CardID);
        playerStats = FindAnyObjectByType<PlayerStats>();
        uiManager = FindAnyObjectByType<UIManager>();
        cardDataBase = FindAnyObjectByType<CardDataBase>();

        cardData = cardDataBase.GetCardById(cardStats.CardID);

        if (cardData != null)
        {
            // 카드 이름과 설명 적용
            cardName.text = cardData.name;
            cardDes.text = ParseText(cardData.description, CreateCardValues());
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return null;

        OnStart();
    }

    // 카드 사용 시 호출 (트라이포드 적용)
    public virtual void SetTripodText()
    {
        Debug.Log("SetTripodText");

        if (cardStats.isCardSkill)
        {
            var values = CreateCardValues();

            uiManager.tripod1Name.text = cardData.options[0].option_name;
            uiManager.tripod1Des.text = ParseText(cardData.options[0].option_description, values);

            uiManager.tripod2Name.text = cardData.options[1].option_name;
            uiManager.tripod2Des.text = ParseText(cardData.options[1].option_description, values);

            uiManager.tripod3Name.text = cardData.options[2].option_name;
            uiManager.tripod3Des.text = ParseText(cardData.options[2].option_description, values);
        }
    }

    // 치환용 딕셔너리 생성
    protected virtual Dictionary<string, string> CreateCardValues()
    {
        var values = new Dictionary<string, string>
        {
            { "base_skill_damage", Mathf.RoundToInt(playerStats.GetPlayerAttack() * cardStats.skill_damage).ToString() },
            { "before_turn", cardStats.beforeActTurn.ToString() }
        };
        return values;
    }

    // 치환 함수
    protected string ParseText(string template, Dictionary<string, string> values)
    {
        return Regex.Replace(template, @"\{(\w+)\}", match =>
        {
            string key = match.Groups[1].Value;
            return values.ContainsKey(key) ? values[key] : match.Value;
        });
    }
}
