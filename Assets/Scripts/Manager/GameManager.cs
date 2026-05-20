using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public UICardTripod UIManager;
    public FieldEffectManager fieldEffectManager;

    public CardList cardList;
    public ObjectClickHandler objectClickHandler;
    public HexTileSelectHandler hexTileSelectHandler;

    public TextMeshProUGUI turnCounter;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;

    public int GameTurn;
    public int ReviveChance;

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
        hexTileManager.OnTurnEnd();
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
        UserCursorOnCards();
    }

    public void CursorOnItems()
    {
        UserCursorOnItems();
    }

    private void UserCursorOnCards()
    {
        objectClickHandler.isPlayerClicked = false;
        BattleItemManager.Instance.ResetSelect();
        HexTileManager.Instance.ResetTileColor();
        player.GetComponent<Player>().PlayerCursor.SetActive(false);
    }

    private void UserCursorOnItems()
    {
        objectClickHandler.isPlayerClicked = false;
        HexTileManager.Instance.ResetTileColor();
        player.GetComponent<Player>().PlayerCursor.SetActive(false);
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        // 1. 현재 활성화된 씬의 정보를 가져옵니다.
        Scene currentScene = SceneManager.GetActiveScene();

        // 2. 해당 씬의 이름을 사용해 다시 로드합니다.
        SceneManager.LoadScene(currentScene.name);
    }

    public void GameOver()
    {
        UIManager.GameOverUI.SetActive(true);
        UIManager.GameOverUI.GetComponent<GameOverUI>().Init(ReviveChance);
        Time.timeScale = 0f;
    }

    public void PlayerDie()
    {
        Player.Instance.stats.KillPlayerInstantly();
    }

    public void Revive()
    {
        if (ReviveChance >= 1)
        {
            ReviveChance--;
            Time.timeScale = 1f;

            PlayerStats stat = player.GetComponent<Player>().stats;
            if (stat.isPlayerDie)
            {
                Player.Instance.Revive();
            }
            UIManager.GameOverUI.SetActive(false);
        }
        else
        {
            StartCoroutine(UIManager.GameOverUI.GetComponent<GameOverUI>().SetWarning());
        }

    }
}