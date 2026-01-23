using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardSkill : MonoBehaviour
{
    public int CardID;

    public GameManager manager;
    public PlayerAnimation playerAnimation;
    public GameObject chainSkill;

    public CardStats baseCardStats;
    public CardStats runtimeCardStats;

    [SerializeField] protected int selectedTripod = 1;
    protected SkillObject skillOption;

    public CardStats Initialize(CardStats stats, int tripodIndex)
    {
        manager = GameManager.Instance;
        playerAnimation = manager.playerAnimation;

        baseCardStats = stats;
        runtimeCardStats = baseCardStats.Clone<CardStats>();
        selectedTripod = tripodIndex;
        SelectTripod(selectedTripod);
        runtimeCardStats.ApplyOption(selectedTripod);
        return runtimeCardStats;
    }

    void Start()
    {
        manager = GameManager.Instance;
        playerAnimation = manager.playerAnimation;
        
    }

    private void SetBaseCardStats(CardStats stat )
    {
        baseCardStats = stat;
        Debug.Log(CardID);
    }

    public CardStats RegisterRuntimeCardStats(CardStats stat)
    {
        runtimeCardStats = Instantiate(baseCardStats);
        return runtimeCardStats;
    }

    public CardStats GetRunTimeCardStats()
    {
        return runtimeCardStats;
    }

    public void SelectTripod(int num)
    {
        selectedTripod = num;
        skillOption = CreateOption(num);
        skillOption?.ApplyOption(this); // 옵션 선택 시 stats 변화
    }


    public virtual IEnumerator Execute()
    {
        Debug.Log("currentSkillExecute");

        if (skillOption == null)
        {
            Debug.LogWarning("스킬 옵션이 선택되지 않았습니다.");
            yield break;
        }

        yield return skillOption.Execute(this);
    }

    public virtual void ApplySkill(bool isBossInRange = false, HexTile tile = null) { }

    protected abstract SkillObject CreateOption(int num);
    public void PlayAnimation(HexTile tile = null) => SkillAnimation(tile);

    public bool HasChainSkill() => chainSkill != null;

    public GameObject GetChainSkill()
    {
        CardStats stat = GetRunTimeCardStats();
        if (stat.HasChainSkill)
        {
            return stat.ChainSkill;
        }
        return null;
    }

    public T GetStats<T>() where T : CardStats
    {
        return runtimeCardStats as T;
    }

    public bool IsBossHit(HexTile tile)
    {
        return HexTileManager.Instance.IsBossTile(tile);
    }

    public bool IsBossHit(List<HexTile> tiles)
    {
        return HexTileManager.Instance.IsBossTile(tiles);
    }

    protected abstract void SkillAnimation(HexTile tile);
}
/*
protected GameObject GetOriginalCard()
    {
        return cardList.GetCard(cardStats.CardID);
    }

    protected void ResetCardStats()
    {
        GameObject originalCard = GetOriginalCard();

        if (originalCard == null)
        {
            Debug.LogError("원본 카드를 찾을 수 없습니다.");
            return;
        }

        CardStats originalStats = originalCard.GetComponent<CardStats>();

        if (originalStats == null)
        {
            Debug.LogError("원본 카드에 CardStats가 없습니다.");
            return;
        }

        if (cardStats == null)
        {
            Debug.LogError("현재 카드에 CardStats가 없습니다.");
            return;
        }

        // CardUtils를 이용해서 stats 복사
        CardUtils.CopyStats(originalStats, cardStats);
    }
    */
