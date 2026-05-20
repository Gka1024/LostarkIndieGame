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
        playerAnimation = Player.Instance.anim;

        baseCardStats = stats;
        runtimeCardStats = baseCardStats.Clone<CardStats>();
        selectedTripod = tripodIndex;
        SelectTripod(selectedTripod);
        runtimeCardStats.ApplyOption(selectedTripod);
        return runtimeCardStats;
    }

    public void SelectTripod(int num)
    {
        Debug.Log("SelectTripod");
        selectedTripod = num;
        skillOption = CreateOption(num);
        skillOption?.ApplyOption(this); // 옵션 선택 시 stats 변화
    }

    public virtual IEnumerator Execute(SkillQueueData data, bool isBossHit)
    {
        Debug.Log("currentSkillExecute");

        if (skillOption == null)
        {
            Debug.LogWarning("스킬 옵션이 선택되지 않았습니다.");
            yield break;
        }

        yield return skillOption.Execute(this, data, isBossHit);
        
    } // 마나 소비 로직 만들것 // 만들음

    public virtual void ApplySkill(bool isBossInRange = false, HexTile tile = null) { }

    protected abstract SkillObject CreateOption(int num);
    public void PlayAnimation(HexTile tile = null) => SkillAnimation(tile);

    public bool HasChainSkill() => chainSkill != null;

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
