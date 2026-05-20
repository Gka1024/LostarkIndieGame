using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TurnStateMachine : MonoBehaviour
{
    public static TurnStateMachine Instance { get; private set; }

    public GameManager manager;
    public QueueManager queueManager;
    public SkillManager skillManager;
    public ObjectClickHandler objectClickHandler;

    public GameObject PlayerTurnObject;
    public GameObject EnemyTurnObject;

    public Boss boss;
    public Player player;

    private bool isLoopStarted = false;

    [SerializeField] private GameTurnState currentState;

    public TaskCompletionSource<bool> chainSkillTCS;

    private bool isPlayerTurnDone = false;

    public bool isNeedToWaitChainTileSelect = false;

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

        // 1. 플레이어가 행동 가능할 때 (카드 사용 등)
        if (player.IsMoveable())
        {
            StartCoroutine(DisplayPlayerTurn(1f));
            GivePlayerCard();

            // 플레이어의 카드 선택이 완료될 때까지 대기
            while (!isPlayerTurnDone) await Task.Yield();
        }
        // 2. 플레이어가 행동 불능일 때 (CC기 등)
        else if (Player.Instance.stats.IsPlayerCrowdControlled())
        {
            CompletePlayerTurn();
        }

        // 3. 큐에 쌓인 액션 처리 (이동 가능 여부와 상관없이 액션이 있다면 실행)
        await ProcessQueueActions();

        PlayerTurnEnd();
        DisablePlayerControl();
        Debug.Log($"{manager.GetTurn()} - 플레이어 턴 종료");
    }

    /// <summary>
    /// 큐에 쌓인 체인 스킬 및 액션을 처리하고, 필요 시 타일 선택을 기다립니다.
    /// </summary>
    private async Task ProcessQueueActions()
    {
        if (!QueueManager.Instance.HasAction()) return;

        // 타일 선택 대기를 위한 TCS 초기화
        ResetChainSkillTCS();

        // 큐 프로세스 시작
        queueManager.ProcessTurn();

        // 타일 선택이 필요한 스킬이라면 선택 완료시까지 대기
        if (isNeedToWaitChainTileSelect)
        {
            Debug.Log("체인 스킬 타일 선택 대기 중...");
            await chainSkillTCS.Task;
        }
    }

    private void ResetChainSkillTCS()
    {
        // 이전 작업이 있다면 취소시키거나 초기화
        chainSkillTCS = new TaskCompletionSource<bool>();
        isNeedToWaitChainTileSelect = false; // 기본값은 false로 세팅 (스킬 내부에서 true로 변경 가정)
    }

    private void GivePlayerCard()
    {
        manager.cardManager.ResetHand();
        manager.cardManager.GiveRandomCard(4);
        manager.cardManager.GiveSpecificCard(141);
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
        manager.cardManager.cardDescriptionUI.OnPointerExit();
    }

    private IEnumerator DisplayPlayerTurn(float time)
    {
        PlayerTurnObject.SetActive(true);

        yield return new WaitForSeconds(time);

        PlayerTurnObject.SetActive(false);
    }

    public void SetChainSkillTCS()
    {
        chainSkillTCS = new TaskCompletionSource<bool>();
        isNeedToWaitChainTileSelect = true;
    }

    // ============= GameTurnState.BossAttack;

    private async Task HandleBossAttack()
    {
        StartCoroutine(DisplayEnemyTurn(1f));
        await Task.Delay(500);
        Debug.Log($"{manager.GetTurn()} - 보스 행동 시작");
        boss.bossController.OnTurnEnd();
    }

    private IEnumerator DisplayEnemyTurn(float time)
    {
        EnemyTurnObject.SetActive(true);

        yield return new WaitForSeconds(time);

        EnemyTurnObject.SetActive(false);
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
