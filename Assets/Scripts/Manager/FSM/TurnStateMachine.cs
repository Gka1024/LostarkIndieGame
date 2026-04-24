using System.Threading.Tasks;
using UnityEngine;

public class TurnStateMachine : MonoBehaviour
{
    public static TurnStateMachine Instance { get; private set; }

    public GameManager manager;
    public QueueManager queueManager;
    public SkillManager skillManager;
    public ObjectClickHandler objectClickHandler;

    public Boss boss;
    public Player player;

    private bool isLoopStarted = false;

    [SerializeField] private GameTurnState currentState;

    public TaskCompletionSource<bool> chainSkillTCS;

    private bool isPlayerTurnDone = false;

    public bool CanPlayerInteract => currentState == GameTurnState.PlayerTurn;


    void Start()
    {
        boss = manager.GetBoss().GetComponent<Boss>();
        player = manager.GetPlayer().GetComponent<Player>();

        Instantiate();
    }

    private void Instantiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }
    }

    public async void StartTurnLoop()
    {
        if (isLoopStarted) return;

        isLoopStarted = true;

        while (true)
        {
            await RunTurnCycle();
        }
    }

    private async Task RunTurnCycle()
    {
        currentState = GameTurnState.BossStartMotion;
        await HandleBossStartMotion();

        currentState = GameTurnState.PlayerTurn;
        await HandlePlayerTurn();

        currentState = GameTurnState.BossAttack;
        await HandleBossAttack();

        currentState = GameTurnState.TurnEnd;
        await HandleTurnEnd();

    }

    // ============= GameTurnState.BossStartMotion;

    private async Task HandleBossStartMotion()
    {

        Debug.Log($"{manager.GetTurn()} - 보스 패턴 예고");
        await Task.Delay(1000);
    }

    // ============= GameTurnState.PlayerTurn;

    private async Task HandlePlayerTurn()
    {
        Debug.Log($"{manager.GetTurn()} - 플레이어 턴 시작");
        manager.TurnStart();

        isPlayerTurnDone = false;
        EnablePlayerControl();

        // 플레이어 턴(카드 선택 등) 대기
        if (player.IsMoveable())
        {
            GivePlayerCard();
            while (!isPlayerTurnDone)
                await Task.Yield();
        }
        else
        {
            if(Player.Instance.stats.GetPlayerDown() || Player.Instance.stats.GetPlayerStun())
            {
                CompletePlayerTurn();
            }
            else if(QueueManager.Instance.HasAction())
            {
                queueManager.ProcessTurn();
            }
            
        }

        PlayerTurnEnd();
        DisablePlayerControl();
        Debug.Log($"{manager.GetTurn()} - 플레이어 턴 종료");
    }

    private void GivePlayerCard()
    {
        manager.cardManager.ResetHand();
        //manager.cardManager.GiveRandomCard(5);
        manager.cardManager.GiveSpecificCard(123);
        manager.cardManager.GiveBasicCard();
    }

    private void EnablePlayerControl()
    {
        objectClickHandler.SetClickAvailable(true);
    }

    private void DisablePlayerControl()
    {
        objectClickHandler.SetClickAvailable(false);
    }

    public void CompletePlayerTurn()
    {
        isPlayerTurnDone = true;
    }

    public void CompletePlayerAction()
    {

    }

    public void PlayerTurnEnd()
    {
        manager.cardManager.DisposeAllCards();
    }

    // ============= GameTurnState.BossAttack;

    private async Task HandleBossAttack()
    {
        await Task.Delay(500);
        Debug.Log($"{manager.GetTurn()} - 보스 행동 시작");
        boss.bossController.OnTurnEnd();
    }

    // ============= GameTurnState.TurnEnd
    private async Task HandleTurnEnd()
    {
        await Task.Delay(500);

        Debug.Log($"{manager.GetTurn()} - 턴 계산");
        manager.TurnEnd();
        manager.ProceedTurnCounter();
    }

}

public enum GameTurnState
{
    BossStartMotion,
    PlayerTurn,
    BossAttack,
    TurnEnd
}
