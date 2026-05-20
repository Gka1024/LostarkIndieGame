using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject button;

    public TextMeshProUGUI GameoverText;
    public TextMeshProUGUI ReviveText;
    public TextMeshProUGUI WarningText;
    public GameObject WarningObject;

    public void Init(int reviveLeft)
    {
        ReviveText.SetText($"남은 부활 횟수 : {reviveLeft}");
    }

    public IEnumerator SetWarning()
    {
        WarningObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        WarningObject.SetActive(false);

    }

    public void ReviveButton()
    {
        GameManager.Instance.Revive();
    }

    public void ResetButton()
    {
        GameManager.Instance.RestartCurrentScene();
    }

}