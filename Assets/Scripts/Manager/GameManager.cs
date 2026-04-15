using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TurnStateMachine turnStateMachine;

    public QueueManager queueManager;
    public HexTileManager hexTileManager;
    public BattleItemManager battleItemManager;
    public SkillManager skillManager;
    public CardManager cardManager;
    public EstherManager estherManager;
    public ObjectManager objectManager;
    public UIManager UIManager;
    public FieldEffectManager fieldEffectManager;


    public PlayerAnimation playerAnimation;

    public CardList cardList;
    public ObjectClickHandler objectClickHandler;
    public HexTileSelectHandler hexTileSelectHandler;

    public TextMeshProUGUI turnCounter;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;

    public int GameTurn;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetBoss()
    {
        return boss;
    }

    public void TurnStart()
    {
        boss.GetComponent<Boss>().ai.OnTurnStart();
        fieldEffectManager.OnTurnStart();
    }

    public void TurnEnd()
    {
        boss.GetComponent<BossStats>().OnTurnEnd();
        player.GetComponent<PlayerStats>().ProcessTurn();

        cardManager.OnTurnEnd();
        battleItemManager.OnTurnEnd();
        estherManager.OnTurnEnd();
        fieldEffectManager.OnTurnEnd();
    }

    public int GetTurn()
    {
        return GameTurn;
    }

    public bool IsPlayerClicked()
    {
        return objectClickHandler.isPlayerClicked;
    }

    public void EndPlayerTurn()
    {
        turnStateMachine.CompletePlayerTurn();
        Debug.Log("PlayerTurnEnd");
    }

    public void ProceedTurnCounter()
    {
        GameTurn++;
        turnCounter.SetText(GameTurn.ToString());
    }

    public void CursorOnCards()
    {
        CancelPlayerClicked();
    }

    public void CursorOnItems()
    {
        CancelPlayerClicked();
    }

    private void CancelPlayerClicked()
    {
        objectClickHandler.isPlayerClicked = false;
        BattleItemManager.Instance.HandsOnCards();
        HexTileManager.Instance.ResetTileColor();
        player.GetComponent<Player>().PlayerCursor.SetActive(false);
    }
}