using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                if (!queueManager.IsFrozen())
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
        // 카드 사용
        // 1. 데이터 불러오기
        currentSkill = currentCard.GetComponent<CardSkill>();
        currentStats = CardList.Instance.GetCardStats(currentSkill.CardID);

        // 2. 플레이어 상황 체크
        if (playerStats.IsPlayerCrowdControlled())
        {
            if (currentSkill.CardID != 100)
            {
                Debug.Log("현재 움직일 수 없습니다. 다른 카드를 사용하세요.");
            }
        }

        // 트라이포드 카드 선택
        // 3. UI 불러오기 및 트라이포드 & 타일 선택

        ShowTripodUI(true);
        ShowCancelButton(true);

        if (playerStats.IsPlayerCrowdControlled())
        {
            if (currentSkill.CardID != 100 && selectedTripod != 2)
            {
                Debug.Log("현재 움직일 수 없습니다. 다른 카드를 사용하세요.");
            }
        }

        yield return new WaitUntil(() => currentState == SkillState.SelectingTile);
        ShowTripodUI(false);

        currentStats.ApplyOption(selectedTripod);

        // 타일 선택
        if (currentStats.needToSelectTile)
        {
            hexTileSelectHandler.StartSelection(currentStats);
            yield return new WaitUntil(() => currentState == SkillState.ExecutingSkill);
        }

        // 4. 큐에 카드스킬 및 체인스킬 데이터 넣기

        if (currentSkill.CardID != 100 || selectedTripod != 1)
        {
            EnqueueData(currentStats, currentSkill.CardID, selectedTripod);
        }

        // 5. 사후 처리

        cardManager.cardList.ApplyCardCooldown(currentStats);

        if (currentStats.isSuperArmor)
        {
            ApplyPlayerSuperArmor(currentStats);
        }

        isCardUsing = false;
        cardManager.DisposeCard(currentCard);
        manager.EndPlayerTurn();
        ShowCancelButton(false);
        currentState = SkillState.Idle;
    }

    private void EnqueueData(CardStats stat, int ID, int tripod)
    {
        EnqueueCardSkill(stat, ID, tripod);

        if (stat.HasChainSkill)
        {
            EnqueueChainSkill(stat, ID, tripod);
        }
    }


    private void ApplyPlayerSuperArmor(bool value = true, int duration = 1)
    {
        if (value)
        {
            playerStats.buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_SUPER_ARMOR, duration));

        }
        else
        {
            playerStats.buffState.RemoveBuff(BuffID_Player.PLAYER_SUPER_ARMOR);
        }
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
        List<HexTile> selectedTiles = hexTileSelectHandler.GetSelectedTiles();
        HexTile selectedTile = selectedTiles.Count > 0 ? hexTileSelectHandler.GetSelectedTile() : null;

        // 큐 데이터 생성
        SkillQueueData data = new SkillQueueData(
            skillId: cardID,
            tripodIndex: tripodIndex,
            mainTile: selectedTile,
            selectedTiles: selectedTiles,
            isChainSkill: false,
            beforeDelay: stats.beforeActTurn,
            afterDelay: stats.afterActTurn
        );

        // 큐에 등록
        QueueManager.Instance.EnqueueSkill(data);
        Debug.Log($"[EnqueueCardSkill] CardID {data.skillId} (트라이포드 {tripodIndex}) 스킬이 큐에 등록됨 {data.beforeDelay} {data.afterDelay}");
    }

    public void EnqueueChainSkill(CardStats stats, int cardID, int tripodIndex)
    {
        Debug.Log("EnqueueChainSkill");

        if (stats == null) return;

        ChainStats chainStats = stats.chainPaths.Find(p => p.tripodIndex == tripodIndex)?.chainStats;

        if (chainStats == null)
        {
            Debug.LogError($"트라이포드 {tripodIndex}에 맞는 ChainStats를 찾을 수 없습니다.");
            return;
        }

        List<HexTile> selectedTiles = hexTileSelectHandler.GetSelectedTiles();
        HexTile selectedTile = selectedTiles.Count > 0 ? hexTileSelectHandler.GetSelectedTile() : null;

        SkillQueueData data = new SkillQueueData(
            skillId: stats.CardID,            // SO에서 가져온 ID
            tripodIndex: tripodIndex,
            isChainSkill: true,
            beforeDelay: 0,
            afterDelay: stats.afterActTurn,
            mainTile: selectedTile,
            selectedTiles: selectedTiles
        );

        queueManager.EnqueueSkill(data);
        Debug.Log($"[EnqueueChainSkill] CardID {cardID} (트라이포드 {tripodIndex}) 스킬이 큐에 등록됨");
    }
    // ========== 스킬 실행 ==========

    public IEnumerator ExecuteCardSkillFromQueue(SkillQueueData data)
    {
        Debug.Log("ExecuteCardSkillFromQueue");

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

        // 3. 대상 판정 및 스킬 실행
        bool bossInRange = tileManager.IsBossTile(data.selectedTiles);
        yield return StartCoroutine(cardSkill.Execute(data, bossInRange));

        // 4. 애니메이션 & 이펙트 - cardSkill.Execute(data, bossInRange)에서 처리함

        // 6. 사후 처리
        CardList.Instance.ApplyCardCooldown(baseStat);
        CardList.Instance.RemoveCardFromHand(baseStat);

        if (!stats.isSuperArmor)
        {
            ApplyPlayerSuperArmor(false);
        }

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
        var chainStat = CardList.Instance.GetChainStats(data.skillId, data.tripodIndex);
        chainSkill.chainStats = chainStat;

        if (chainStat.needToSelectTile)
        { // 타일 선택이 필요하다면 
            currentState = SkillState.SelectingTile;
            hexTileSelectHandler.StartSelection(chainStat);

            TurnStateMachine.Instance.SetChainSkillTCS();

            yield return new WaitUntil(() => currentState == SkillState.ExecutingSkill);

            TurnStateMachine.Instance.chainSkillTCS?.SetResult(true);

            data.mainTile = GetSelectedTile();
            data.selectedTiles = GetSelectedTiles();
        }

        // 3. 대상 판정 및 효과 처리
        bool bossInRange = tileManager.IsBossTile(data.selectedTiles);
        yield return StartCoroutine(chainSkill.ExecuteChain(data, bossInRange));


        // 6. 사후 처리 
        ApplyPlayerSuperArmor(false);
        Destroy(chainGO);

    }

    // ========== 스킬 적용 ==========

    public void ApplyBossSkills(CardStats stat)
    {
        float damage = stat.skill_damage;
        float stagger = stat.stagger;
        float identityGain = stat.identityGain;

        BossDamageData data = DamageSystem.Instance.ProcessDamage(new BossDamageData(damage, stagger));

        boss.GetComponent<Boss>().bossController.GetBossDamageData(data);
        GivePlayerIdentity(identityGain);
    }

    public void ApplyBossSkills(ChainStats stat)
    {
        float damage = stat.skill_damage;
        float stagger = stat.stagger;

        BossDamageData data = DamageSystem.Instance.ProcessDamage(new BossDamageData(damage, stagger));
        boss.GetComponent<Boss>().bossController.GetBossDamageData(data);
    }

    public void ApplyBossSkills(BossDamageData data)
    {
        BossDamageData processedData = DamageSystem.Instance.ProcessDamage(data);
        boss.GetComponent<Boss>().bossController.GetBossDamageData(processedData);
    }

    public void GivePlayerIdentity(float identity)
    {
        playerStats.AddPlayerIdentity(identity);
    }

    public void ApplyBossDebuff(BossBuff buff)
    {
        boss.GetComponent<Boss>().bossController.AddBuff(buff);
    }

    public void PlayAnimaion(CardSkill skill, PlayerWeapon playerWeapon, HexTile tile)
    {
        Debug.Log("SkillAnimation");
        player.GetComponent<Player>().anim.ChangeWeapon(playerWeapon);
        skill.PlayAnimation(tile);
    }

    // ========== 유틸 함수 ==========
    public void SelectTripod(int tripodIndex)
    {
        isTripodSelected = true;
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

    public bool CheckPlayerMoveable()
    {
        if (queueManager.IsFrozen()) return false; // 후딜레이가 있을 때
        if (isCardUsing) return false;

        if (playerStats.IsPlayerCrowdControlled()) return false;
        if (playerStats.IsPlayerCrowdControlled()) return false;

        return true;
    }

}
