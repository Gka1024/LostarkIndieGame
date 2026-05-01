using TMPro;
using UnityEngine;

public class CardDescriptionUI : MonoBehaviour
{
    public GameObject descriptionCard;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescBeforeDelay;
    public TextMeshProUGUI cardDescAfterDelay;
    public TextMeshProUGUI cardDescStagger;
    public TextMeshProUGUI cardDescDestoy;

    public void SetCardText(int cardID)
    {
        CardStats stat = CardList.Instance.GetCardStats(cardID);
        CardData data = GameManager.Instance.cardManager.cardDataBase.GetCardById(cardID);
        
        cardName.SetText(data.name);
        cardDescBeforeDelay.SetText($"선딜레이 : {stat.beforeActTurn} 턴");
        cardDescAfterDelay.SetText($"후딜레이 : {stat.afterActTurn} 턴");
        cardDescStagger.SetText($"무력화 : {stat.stagger}");
        cardDescDestoy.SetText($"파괴 : {stat.destroy}");
    }

    public void OnPointerEnter()
    {
        descriptionCard.SetActive(true);
    }

     public void OnPointerExit()
    {
        descriptionCard.SetActive(false);
    }
}