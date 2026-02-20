using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public GameManager manager;

    public QueueManager queueManager;
    public HexTileManager tileManager;
    public CardManager cardManager;
    public HexTileSelectHandler hexTileSelectHandler;
    public BattleItemManager battleItemManager;
    public PlayerStats playerStats;

    public ChainSkillList chainSkillList;

    public GameObject boss;
    public GameObject player;

    private enum SkillState
    {
        Idle,
        SelectingTripod,
        SelectingTile,
        ExecutingSkill
    }

    [SerializeField] private SkillState currentState = SkillState.Idle;

    [SerializeField] private bool isCardUsing;
    [SerializeField] private GameObject currentCard;
    [SerializeField] private CardSkill currentSkill;
    CardStats currentStats;

    [SerializeField] private bool isCharacterFrozen;
    [SerializeField] private int beforeDelayTurns;
    [SerializeField] private int afterDelayTurns;

    public List<HexTile> selectedTiles;
    public HexTile selectedTile;

    private bool isTripodSelected = false;
    private int selectedTripod = -1;

    private Coroutine skillCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        switch (currentState)
        {
            case SkillState.Idle:
                if (isCardUsing)
                    currentState = SkillState.SelectingTripod;
                break;

            case SkillState.SelectingTripod:
                if (isTripodSelected)
                    currentState = SkillState.SelectingTile;
                break;

            case SkillState.SelectingTile:
                if (hexTileSelectHandler.isTileSelected)
                    currentState = SkillState.ExecutingSkill;
                break;

            case SkillState.ExecutingSkill:
                if (!queueManager.HasActions())
                    currentState = SkillState.Idle;
                break;

        }
    }

    // ========== 스킬 시퀀스 시작 ==========
    public void StartSkillSequence(GameObject card)
    {
        currentCard = card;
        isTripodSelected = false;
        isCardUsing = true;
        currentState = SkillState.SelectingTripod;
        skillCoroutine = StartCoroutine(SkillSequenceRoutine());
    }

    private IEnumerator SkillSequenceRoutine()
    {
        // 1. 데이터 불러오기
        currentSkill = currentCard.GetComponent<CardSkill>();
        currentStats = CardList.Instance.GetCardStats(currentSkill.CardID);

        // 2. 플레이어 상황 체크
        if (playerStats.GetPlayerDown() || playerStats.GetPlayerSilenced() || playerStats.GetPlayerStun())
        {
            if (currentSkill.CardID != 001)
            {
                Debug.Log("현재 움직일 수 없습니다. 다른 카드를 사용하세요.");
            }
        }

        // 3. UI 불러오기 및 트라이포드 & 타일 선택

        ShowTripodUI(true);
        ShowCancelButton(true);

        //currentStats.ApplyOption(selectedTripod);

        yield return new WaitUntil(() => currentState == SkillState.SelectingTile);

        ShowTripodUI(false);
        if (currentStats.needToSelectTile)
        {
            hexTileSelectHandler.StartSelection(currentStats);
            yield return new WaitUntil(() => currentState == SkillState.ExecutingSkill);
        }

        // 4. 큐에 카드스킬 및 체인스킬 데이터 넣기

        EnqueueCardSkill(currentStats, currentSkill.CardID, selectedTripod);

        if (currentStats.HasChainSkill)
        {
            EnqueueChainSkill(currentStats, currentSkill.CardID, selectedTripod);
        }

        // 5. 사후 처리

        cardManager.cardList.ApplyCardCooldown(currentStats);

        if (currentStats.isSuperArmor)
        {
            ApplyPlayerSuperArmor(currentStats);
        }

        cardManager.DisposeCard(currentCard);
        manager.EndPlayerTurn();
        ShowCancelButton(false);
        isCardUsing = false;
        currentState = SkillState.Idle;
    }

    private void ApplyPlayerSuperArmor(bool value = true)
    {
        playerStats.ApplyPlayerSuperArmor(value);
    }

    // ========== 큐 등록 ==========
    private void EnqueueCardSkill(CardStats stats, int cardID, int tripodIndex)
    {
        if (stats == null)
        {
            Debug.LogError("EnqueueCardSkill: stats가 null입니다.");
            return;
        }

        // 타일 정보 가져오기
        selectedTiles = hexTileSelectHandler.GetSelectedTiles();
        selectedTile = selectedTiles.Count > 0 ? hexTileSelectHandler.GetSelectedTile() : null;

        // 큐 데이터 생성
        SkillQueueData data = new SkillQueueData(
            skillId: cardID,
            tripodIndex: tripodIndex,
            mainTile: selectedTile,
            selectedTiles: selectedTiles,
            damage: stats.skill_damage,
            stagger: stats.stagger,
            identity: stats.identityGain,
            manaCost: stats.manaUse,
            isChainSkill: false
        );

        // 큐에 등록
        QueueManager.Instance.EnqueueSkill(data, stats.beforeActTurn, stats.afterActTurn);
        Debug.Log($"[EnqueueCardSkill] CardID {cardID} (트라이포드 {tripodIndex}) 스킬이 큐에 등록됨");
    }

    public void EnqueueChainSkill(CardStats stats, int cardID, int tripodIndex)
    {
        if (stats == null)
        {
            Debug.LogError("EnqueueChainSkill: stats가 null입니다.");
            return;
        }

        Debug.Log($"[EnqueueChainSkill] CardID {cardID} (트라이포드 {tripodIndex}) 스킬이 큐에 등록됨");

        ChainSkill chain = stats.ChainSkill.GetComponent<ChainSkill>();
        ChainStats chainStats = chain.chainStats;

        HexTile selectedTile = stats.needToSelectTile ? null : chain.GetTargetTile(GetSelectedTile());
        List<HexTile> selectedTiles = stats.needToSelectTile ? null : GetSelectedTiles();

        SkillQueueData data = new SkillQueueData(
           skillId: cardID,
            tripodIndex: tripodIndex,
            mainTile: selectedTile,
            selectedTiles: selectedTiles,
            damage: chainStats.skill_damage,
            stagger: chainStats.stagger,
            identity: chainStats.identityGain,
            manaCost: 0,
            isChainSkill: true
        );

        queueManager.EnqueueSkill(data, 0, chainStats.afterDelay);

    }
    // ========== 스킬 실행 ==========

    public IEnumerator ExecuteSkillFromQueue(SkillQueueData data)
    {
        // 1. 데이터 가져오기
        var baseStat = CardList.Instance.GetCardStats(data.skillId);
        var prefab = CardList.Instance.GetCardByID(data.skillId);
        if (baseStat == null || prefab == null)
        {
            Debug.LogError($"스킬 데이터를 찾을 수 없음: {data.skillId}");
            yield break;
        }

        // 2. 프리팹 인스턴스 생성
        var skillGO = Instantiate(prefab);
        var cardSkill = skillGO.GetComponent<CardSkill>();
        CardStats stats = cardSkill.Initialize(baseStat, data.tripodIndex);

        // 3. 대상 판정 및 효과 처리
        bool bossInRange = tileManager.IsBossTile(data.selectedTiles);

        if (bossInRange)
        {
            ApplyBossSkills(stats);

            GivePlayerIdentity(stats.identityGain);
        }

        // 4. 애니메이션 & 이펙트
        player.GetComponent<Player>().anim.ChangeWeapon(baseStat.playerWeapon);
        cardSkill.PlayAnimation(data.mainTile);

        // 5. 실행 (코루틴 대기)
        yield return StartCoroutine(cardSkill.Execute());

        // 6. 사후 처리
        playerStats.UseMana(data.manaCost);
        CardList.Instance.ApplyCardCooldown(baseStat);
        CardList.Instance.RemoveCardFromHand(baseStat);

        if (!stats.HasChainSkill)
        {
            ApplyPlayerSuperArmor(false);
        }

        // 체인 스킬
        //if (data.isChainSkill) StartCoroutine(ExecuteChainSkill(data));

        Destroy(skillGO);
    }

    public IEnumerator ExecuteChainSkillFromQueue(SkillQueueData data)
    {
        Debug.Log($"체인 스킬 사용: {data.skillId}");

        // 1. 데이터 가져오기
        var chainSkillData = CardList.Instance.GetChainSkills(data.skillId, data.tripodIndex);
        if (chainSkillData == null)
        {
            Debug.LogError($"스킬 데이터를 찾을 수 없음: {data.skillId}");
            yield break;
        }

        // 2. 프리팹 인스턴스 생성
        var chainGO = Instantiate(chainSkillData);
        var chainSkill = chainGO.GetComponent<ChainSkill>();

        // 2.5 트라이포드 적용 및 타일 선택 
        chainSkill.SetTripod(data.tripodIndex);
        var chainStat = chainSkill.chainStats;

        if (chainStat.needToSelectTile)
        { // 타일 선택이 필요하다면 
            currentState = SkillState.SelectingTile;
            hexTileSelectHandler.StartSelection(chainStat);
            yield return new WaitUntil(() => currentState == SkillState.ExecutingSkill);

            TurnStateMachine.Instance.chainSkillTCS?.SetResult(true);

            data.mainTile = GetSelectedTile();
            data.selectedTiles = GetSelectedTiles();
        }

        // 3. 대상 판정 및 효과 처리
        bool bossInRange = tileManager.IsBossTile(data.selectedTiles);

        if (bossInRange)
        {
            ApplyBossSkills(chainStat);
            GivePlayerIdentity(chainStat.identityGain);
        }

        // 4. 애니메이션 & 이펙트
        // chainSkill.PlayAnimation(data.mainTile);

        // 5. 실행 (코루틴 대기)
        Debug.Log("ExecuteChain");
        yield return StartCoroutine(chainSkill.ExecuteChain(data));

        // 6. 사후 처리 
        ApplyPlayerSuperArmor(false);
        Destroy(chainGO);
    }

    public void ApplyBossSkills(CardStats stat)
    {
        float damage = stat.skill_damage;
        float stagger = stat.stagger;

        boss.GetComponent<Boss>().bossController.GetBossDamageData(new BossDamageData(damage, stagger));
    }

    public void ApplyBossSkills(ChainStats stat)
    {
        float damage = stat.skill_damage;
        float stagger = stat.stagger;

        boss.GetComponent<Boss>().bossController.GetBossDamageData(new BossDamageData(damage, stagger));
    }

    public void GivePlayerIdentity(float identity)
    {
        playerStats.AddPlayerIdentity(identity);
    }

    public void MakeBossTaunt(int tauntTurns)
    {
        BossDebuff tauntDebuffs = new BossDebuff(DebuffType.Taunt, 1, tauntTurns, 1);

        boss.GetComponent<BossStatus>().AddBossDebuff(tauntDebuffs);
    }

    // ========== 유틸 함수 ==========
    public void SelectTripod(int tripodIndex)
    {
        isTripodSelected = true;

        currentStats.ApplyOption(tripodIndex);
        selectedTripod = tripodIndex;
    }

    public List<HexTile> GetSelectedTiles() => hexTileSelectHandler.selectedTiles;
    public HexTile GetSelectedTile() => hexTileSelectHandler.selectedTile;
    public int GetTripod() => selectedTripod;

    private void ShowTripodUI(bool show) => cardManager.TripodButton.SetActive(show);
    private void ShowCancelButton(bool show) => cardManager.TripodCancelButton.SetActive(show);

    public void CancelTripod() => ResetSkillState();

    private void ResetSkillState()
    {
        if (skillCoroutine != null)
            StopCoroutine(skillCoroutine);

        hexTileSelectHandler.CancelSelection();
        isTripodSelected = false;
        selectedTripod = -1;
        isCardUsing = false;
        currentCard = null;
        currentState = SkillState.Idle;

        ShowTripodUI(false);
        ShowCancelButton(false);
    }

    public bool CanMove()
    {
        if (queueManager.IsFrozen()) return false;
        if (queueManager.HasActions()) return false; // 후딜레이가 있을 때
        if (isCardUsing) return false;

        if (playerStats.GetPlayerDown()) return false;
        if (playerStats.GetPlayerStun()) return false;

        return true;
    }

}
