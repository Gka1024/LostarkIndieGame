using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager Instance { get; private set; }

    [Header("References")]
    public TurnStateMachine turnStateMachine;
    [SerializeField] private SkillManager skillManager;

    [Header("Status")]
    [SerializeField] private Queue<SkillQueueData> actionQueue = new();
    [SerializeField] private SkillQueueData activeSkill = null;
    [SerializeField] private int remainingBeforeDelay;
    [SerializeField] private int remainingAfterDelay;
    [SerializeField] private bool isCharacterFrozen;
    private bool isProcessing = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Clear();
    }

    public void EnqueueSkill(SkillQueueData data)
    {
        Debug.Log($"data.isChain? : {data.isChainSkill}");
        actionQueue.Enqueue(data);
    }

    public void ProcessTurn()
    {
        if (isProcessing) return;

        // 1. 후딜레이 처리
        if (TryConsumeAfterDelay()) return;

        // 2. 현재 실행할 스킬이 없다면 큐에서 꺼내기
        if (activeSkill == null)
        {
            if (!TryDequeueNextSkill()) return;
        }

        // 3. 선딜레이 처리
        if (TryConsumeBeforeDelay()) return;

        // 4. 스킬 실행
        StartCoroutine(ExecuteSkillSequence());
    }

    private bool TryConsumeAfterDelay()
    {
        if (remainingAfterDelay <= 0) return false;

        remainingAfterDelay--;
        Debug.Log($"후딜레이 소모 중... 남은 턴: {remainingAfterDelay}");
        isCharacterFrozen = true;
        ActionEnd(); // 딜레이 소모 시 턴 종료
        return true;
    }

    private bool TryConsumeBeforeDelay()
    {
        if (remainingBeforeDelay <= 0) return false;

        remainingBeforeDelay--;
        Debug.Log($"선딜레이 소모 중... 남은 턴: {remainingBeforeDelay}");
        isCharacterFrozen = true;
        ActionEnd(); // 딜레이 소모 시 턴 종료
        return true;
    }

    private bool TryDequeueNextSkill()
    {
        if (actionQueue.Count == 0)
        {
            isCharacterFrozen = false;
            ActionEnd(); // 모든 큐 소진 시 턴 종료
            return false;
        }

        activeSkill = actionQueue.Dequeue();

        // 여기서 딜레이 변수를 스킬 데이터로부터 초기화 (중요!)
        remainingBeforeDelay = activeSkill.beforeDelay;
        remainingAfterDelay = activeSkill.afterDelay;

        return true;
    }

    private IEnumerator ExecuteSkillSequence()
    {
        isProcessing = true;
        isCharacterFrozen = false;

        Debug.Log($"스킬 실행 시작: {activeSkill.skillId} :: {activeSkill.isChainSkill}");

        // 스킬 로직 분기 처리
        if (activeSkill.isChainSkill)
        {
            turnStateMachine.chainSkillTCS = new TaskCompletionSource<bool>();
            yield return StartCoroutine(skillManager.ExecuteChainSkillFromQueue(activeSkill));
        }
        else
        {
            yield return StartCoroutine(skillManager.ExecuteCardSkillFromQueue(activeSkill));
        }

        activeSkill = null;
        isProcessing = false;

        Debug.Log("스킬 연출 종료");
        ActionEnd();
    }

    private void ActionEnd()
    {
        GameManager.Instance.EndPlayerTurn();
    }

    public void Clear()
    {
        StopAllCoroutines();
        actionQueue.Clear();
        activeSkill = null;
        remainingBeforeDelay = 0;
        remainingAfterDelay = 0;
        isProcessing = false;
        isCharacterFrozen = false;
    }

    public bool IsFrozen() => isCharacterFrozen || actionQueue.Count != 0;
    public int GetQueueCount() => actionQueue.Count;

    public void GetQueueCountForDebug()
    {
        Debug.Log($"Queue: {GetQueueCount()} - beforeActTurn : {remainingBeforeDelay} afterActTurn : {remainingBeforeDelay}");
    }
}

// 데이터 클래스는 변경 없음
[System.Serializable]
public class SkillQueueData
{
    public int skillId;
    public int tripodIndex;
    public HexTile mainTile;
    public List<HexTile> selectedTiles;
    public bool isChainSkill;
    public int beforeDelay;
    public int afterDelay;

    public SkillQueueData(
        int skillId,
        int tripodIndex,
        bool isChainSkill,
        int beforeDelay = 0,
        int afterDelay = 0,
        HexTile mainTile = null,
        List<HexTile> selectedTiles = null
    )
    {
        this.skillId = skillId;
        this.tripodIndex = tripodIndex;
        this.isChainSkill = isChainSkill;
        this.beforeDelay = beforeDelay;
        this.afterDelay = afterDelay;
        this.mainTile = mainTile;
        this.selectedTiles = selectedTiles ?? new List<HexTile>();
    }
}