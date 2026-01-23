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

    [SerializeField] private GameTurnState currentState;

    public TaskCompletionSource<bool> chainSkillTCS;

    private bool isPlayerTurnDone = false;
    private bool isPlayerActionDone = false;

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

    // =============

    private async Task HandleBossStartMotion()
    {
        boss.ai.OnTurnStart();
        Debug.Log($"보스 모션 시작 : {manager.GetTurn()}");
        await Task.Delay(1000);
    }

    // =============

    private async Task HandlePlayerTurn()
    {
        Debug.Log($"플레이어 턴 시작 : {manager.GetTurn()}");

        isPlayerTurnDone = false;

        GivePlayerCard();
        EnablePlayerControl();

        if (skillManager.CanMove())
        {
            while (!isPlayerTurnDone)
                await Task.Yield();
        }
        else
        {
            // 스킬 사용했으므로 바로 종료
            CompletePlayerTurn();
        }

        queueManager.ProcessTurn();

        if (chainSkillTCS != null)
        {
            Debug.Log("체인스킬 실행 대기 중...");
            await chainSkillTCS.Task;
            chainSkillTCS = null;
            Debug.Log("체인스킬 실행 완료!");
        }

        PlayerTurnEnd();
        DisablePlayerControl();
        Debug.Log($"플레이어 턴 종료 : {manager.GetTurn()}");

    }

    private void GivePlayerCard()
    {
        manager.cardManager.ResetHand();
        manager.cardManager.GiveCard(5);
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
        isPlayerActionDone = true;
    }

    public void PlayerTurnEnd()
    {
        manager.cardManager.DisposeAllCards();
    }

    // =============

    private async Task HandleBossAttack()
    {
        await Task.Delay(1000);
        Debug.Log($"보스 행동 시작 : {manager.GetTurn()}");
        boss.bossController.OnTurnEnd();
    }

    // =============
    private async Task HandleTurnEnd()
    {
        await Task.Delay(1000);

        Debug.Log($"턴 계산 : {manager.GetTurn()}");
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
