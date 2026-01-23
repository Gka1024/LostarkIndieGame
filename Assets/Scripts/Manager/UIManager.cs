using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UtilUI;
    public GameObject CardUI;
    public GameObject CardTripodUI;
    public GameObject BossUi;
    public GameObject PlayerUI;
    public GameObject BattleItemUI;
    public GameObject EstherUI;

    public TextMeshProUGUI tripod1Name;
    public TextMeshProUGUI tripod1Des;
    public TextMeshProUGUI tripod2Name;
    public TextMeshProUGUI tripod2Des;
    public TextMeshProUGUI tripod3Name;
    public TextMeshProUGUI tripod3Des;

    public void AddEsther(float value)
    {
        EstherUI.GetComponent<EstherManager>().AddEstherValue(value);
    }

}
