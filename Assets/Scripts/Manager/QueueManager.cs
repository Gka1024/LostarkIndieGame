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
    [SerializeField] private bool activeSkillEmpty = true;

    // 현재 활성화된 스킬의 딜레이를 관리
    [SerializeField] private int beforeDelay;
    [SerializeField] private int afterDelay;
    [SerializeField] private int chainDelay;

    [SerializeField] private bool isCharacterFrozen;
    [SerializeField] private bool isProcessing = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Clear();
    }

    public void EnqueueSkill(SkillQueueData data)
    {
        isProcessing = true;

        beforeDelay += data.beforeDelay;
        afterDelay += data.isChainSkill ? 0 : data.afterDelay;
        chainDelay += data.isChainSkill ? data.afterDelay : 0; // 예시 로직
        actionQueue.Enqueue(data);

        Debug.Log($"Queuemanager - Enqueue {beforeDelay} {afterDelay} {chainDelay}");
    }

    public void ProcessTurn()
    {
        if (beforeDelay > 0)
        {
            ConsumeDelay(0);
            return;
        }

        if (actionQueue.Count > 0)
        {
            TryDequeueNextSkill();
        }

        if (!activeSkillEmpty && activeSkill.isChainSkill == false)
        {
            StartCoroutine(ExecuteSkillSequence());
            return;
        }

        if (afterDelay > 0)
        {
            ConsumeDelay(1);
            return;
        }

        if (!activeSkillEmpty && activeSkill.isChainSkill)
        {
            StartCoroutine(ExecuteSkillSequence());
            return;
        }

        if (chainDelay > 0)
        {
            ConsumeDelay(2);
            return;
        }

    }

    private void ConsumeDelay(int index)
    {
        isCharacterFrozen = true;
        switch (index)
        {
            case 0: beforeDelay--; Debug.Log($"선딜레이 소모: {beforeDelay}"); break;
            case 1: afterDelay--; Debug.Log($"후딜레이 소모: {afterDelay}"); break;
            case 2: chainDelay--; Debug.Log($"체인딜레이 소모: {chainDelay}"); break;
        }

        if (isProcessing) CheckProcess();
        ActionEnd();
    }

    private void CheckProcess()
    {
        if (beforeDelay == 0 && afterDelay == 0 && chainDelay == 0 && actionQueue.Count == 0 && activeSkillEmpty)
        {
            isCharacterFrozen = false;
            isProcessing = false;
        }
    }

    private bool TryDequeueNextSkill()
    {
        if (actionQueue.Count == 0) return false;

        activeSkill = actionQueue.Dequeue();
        activeSkillEmpty = false;

        return true;
    }

    private IEnumerator ExecuteSkillSequence()
    {
        if (activeSkill.skillId == 0) yield return 0;

        if (activeSkill.isChainSkill)
        {
            turnStateMachine.chainSkillTCS = new TaskCompletionSource<bool>();
            yield return StartCoroutine(skillManager.ExecuteChainSkillFromQueue(activeSkill));
        }
        else
        {
            yield return StartCoroutine(skillManager.ExecuteCardSkillFromQueue(activeSkill));
        }

        activeSkill = null; // 스킬이 끝났으므로 비움
        activeSkillEmpty = true;

        if (isProcessing) CheckProcess();
        ActionEnd();
    }

    private void ActionEnd() => GameManager.Instance.EndPlayerTurn();

    public void Clear()
    {
        StopAllCoroutines();
        actionQueue.Clear();
        activeSkill = null;
        activeSkillEmpty = true;
        beforeDelay = 0;
        afterDelay = 0;
        chainDelay = 0;
        isProcessing = false;
        isCharacterFrozen = false;
    }

    public bool IsFrozen() => isCharacterFrozen || actionQueue.Count != 0;
    public int GetQueueCount() => actionQueue.Count;

    public void GetQueueCountForDebug()
    {
        Debug.Log($"Queue: {HasAction()} - beforeActTurn : {beforeDelay} afterActTurn : {afterDelay}");
    }

    public bool HasAction()
    {
        bool value = false;

        if (beforeDelay > 0) return true;
        if (afterDelay > 0) return true;
        if (chainDelay > 0) return true;
        if (actionQueue.Count > 0) return true;
        if (isProcessing) return true;

        return value;
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