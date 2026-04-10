using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager Instance { get; private set; }

    public TurnStateMachine turnStateMachine;
    [SerializeField] private SkillManager skillManager;

    private Queue<SkillQueueData> actionQueue = new();

    // 현재 제어 중인 스킬과 남은 딜레이 변수
    [Header("Current Status")]
    [SerializeField] private SkillQueueData activeSkill = null;
    [SerializeField] private int remainingBeforeDelay;
    [SerializeField] private int remainingAfterDelay;

    [Header("Flags")]
    [SerializeField] private bool isCharacterFrozen;
    private bool isProcessing = false; // 코루틴 실행 중인지 확인

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Clear();
    }

    public void EnqueueSkill(SkillQueueData data)
    {
        actionQueue.Enqueue(data);
    }

    public void Actionqueuesize()
    {
        Debug.Log(actionQueue.Count());
    }

    public void ProcessTurn()
    {
        if (isProcessing) return;

        if (remainingAfterDelay > 0)
        {
            remainingAfterDelay--;
            Debug.Log($"후딜레이(경직) 소모 중... 남은 턴: {remainingAfterDelay}");
            isCharacterFrozen = true;
            ActionEnd();
            return;
        }

        if (activeSkill == null)
        {
            if (actionQueue.Count == 0)
            {
                isCharacterFrozen = false;
                ActionEnd();
                return;
            }

            activeSkill = actionQueue.Dequeue();
            remainingBeforeDelay = activeSkill.beforeDelay;
            remainingAfterDelay = activeSkill.afterDelay;
        }

        if (remainingBeforeDelay > 0)
        {
            remainingBeforeDelay--;
            Debug.Log($"선딜레이(캐스팅) 소모 중... 남은 턴: {remainingBeforeDelay}");
            isCharacterFrozen = true;
            ActionEnd();
            return;
        }

        StartCoroutine(ExecuteSkillSequence());
    }

    private IEnumerator ExecuteSkillSequence()
    {
        isProcessing = true;
        isCharacterFrozen = false; // 실행 중엔 애니메이션을 보여줘야 하므로 frozen 해제

        Debug.Log($"스킬 실행 시작: {activeSkill.skillId}");

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
        // 턴 종료 알림
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

    public bool IsFrozen() => isCharacterFrozen;
}


[System.Serializable]
public class SkillQueueData
{
    public int skillId;           // 스킬 식별자
    public int tripodIndex;       // 선택된 트리포드 번호
    public HexTile mainTile;      // 클릭한 메인 타일
    public List<HexTile> selectedTiles; // 범위 내 선택된 타일들
    public bool isChainSkill;     // 체인 스킬 여부

    public int beforeDelay;       // 실행 전 대기 턴/시간
    public int afterDelay;        // 실행 후 대기 턴/시간

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