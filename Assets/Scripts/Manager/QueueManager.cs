using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager Instance { get; private set; }

    public TurnStateMachine turnStateMachine;

    [SerializeField] private SkillManager skillManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private HexTileManager tileManager;

    [SerializeField] private GameObject Boss;

    private Queue<object> actionQueue = new();
    private CardSkill sideSkill;
    [SerializeField] private bool isCharacterFrozen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnqueueSkill(SkillQueueData data, int beforeDelay, int afterDelay)
    {
        Debug.Log($"EnqueueSkill - beforedelay : {beforeDelay} || afterdelay : {afterDelay}");
        for (int i = 0; i < beforeDelay; i++) actionQueue.Enqueue(null);
        actionQueue.Enqueue(data);
        for (int i = 0; i < afterDelay; i++) actionQueue.Enqueue(null);
    }

    public void ProcessTurn()
    {
        if (actionQueue.Count == 0)
        {
            isCharacterFrozen = false;
            ActionEnd();
            return;
        }

        object currentAction = actionQueue.Dequeue();

        if (currentAction == null && HasActions())
        { // 후딜레이가 있을 경우
            Debug.Log("Turn Action is null");
            isCharacterFrozen = true;
        }
        else
        {
            if (currentAction is SkillQueueData data)
            {
                if (data.isChainSkill)
                {
                    turnStateMachine.chainSkillTCS = new TaskCompletionSource<bool>();
                    StartCoroutine(ExecuteChainSkillFlow(data));
                }
                else
                {
                    StartCoroutine(ExecuteCardSkillFlow(data));
                }
            }
        }

        Debug.Log("Turn Action End");
        ActionEnd();
    }

    private IEnumerator ExecuteCardSkillFlow(SkillQueueData data)
    {
        StartCoroutine(skillManager.ExecuteSkillFromQueue(data));

        yield return null;
    }

    private IEnumerator ExecuteChainSkillFlow(SkillQueueData data)
    {
        StartCoroutine(skillManager.ExecuteChainSkillFromQueue(data));

        yield return null;
    }


    public void Clear() => actionQueue.Clear();
    public bool IsFrozen() => isCharacterFrozen;
    public bool HasActions() => actionQueue.Count > 0;

    public void DebugForQueueSize()
    {
        if (HasActions())
        {
            foreach (var obj in actionQueue)
            {
                Debug.Log(obj);
            }
        }
    }

    private void ActionEnd()
    {
        GameManager.Instance.EndPlayerTurn();
    }
}

[System.Serializable]
public class SkillQueueData
{
    public int skillId;                   // 어떤 스킬인지 식별자
    public int tripodIndex;                 // 트라이포드 선택 결과
    public HexTile mainTile;        // 메인 타일 위치
    public List<HexTile> selectedTiles; // 선택한 타일들의 위치
    public float damage;
    public float stagger;
    public float identity;
    public float manaCost;
    public bool isChainSkill;

    public SkillQueueData(
        int skillId,
        int tripodIndex,
        
        float damage,
        float stagger,
        float identity,

        float manaCost,
        bool isChainSkill,
        
        HexTile mainTile = null,
        List<HexTile> selectedTiles= null
    )
    {
        this.skillId = skillId;
        this.tripodIndex = tripodIndex;
        this.mainTile = mainTile;
        this.selectedTiles = selectedTiles;
        this.damage = damage;
        this.stagger = stagger;
        this.identity = identity;
        this.manaCost = manaCost;
        this.isChainSkill = isChainSkill;
    }
}