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

    public PlayerAnimation playerAnimation;

    public CardList cardList;
    public GameObject objectClickHandler;
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

    public void TurnEnd()
    {
        boss.GetComponent<BossStats>().ProceedTurn();
        player.GetComponent<PlayerStats>().ProcessTurn();

        cardManager.OnTurnEnd();
        battleItemManager.OnTurnEnd();
        estherManager.OnTurnEnd();
    }

    public int GetTurn()
    {
        return GameTurn;
    }

    public bool IsPlayerClicked()
    {
        ObjectClickHandler objectClickHandlerSC = objectClickHandler.GetComponent<ObjectClickHandler>();
        return objectClickHandlerSC.isPlayerClicked;
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

    public void ResetTileColor()
    {
        hexTileManager.ResetTileColor();
    }

}

