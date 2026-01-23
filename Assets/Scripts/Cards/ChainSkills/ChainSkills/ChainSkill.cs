using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainSkill : MonoBehaviour
{
    public int CardID;
    public int CardTripodNum;

    public ChainStats chainStats;

    public GameManager manager;
    public SkillManager skillManager;
    public CardList cardList;

    public GameObject Player;
    public PlayerStats playerStats;
    public PlayerMove playerSC;
    public GameObject Boss;

    public CardSkill baseCardSkill;
    public int tripodNum;


    protected virtual void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();
        skillManager = FindAnyObjectByType<SkillManager>();
        cardList = FindAnyObjectByType<CardList>();
        Player = manager.GetPlayer();
        playerStats = Player.GetComponent<PlayerStats>();
        playerSC = Player.GetComponent<PlayerMove>();
        Boss = manager.GetBoss();
    }

    public virtual void InitFromCardSkill(CardSkill skill)
    {
        var card = cardList.GetCardByID(skill.GetComponent<CardStats>().CardID);
        //SetTripod(skillManager.GetTripod());
        if (card != null)
        {
            baseCardSkill = card.GetComponent<CardSkill>();
        }
        else
        {
            Debug.LogWarning("CardSkill을 초기화하는데 실패했습니다. CardList에서 ID를 찾을 수 없습니다.");
        }
    }

    public abstract IEnumerator ExecuteChain(SkillQueueData data);

    protected CardStats GetOriginalCardStats(int index = 0)
    {
        if(index == 0)
        {
            index = this.CardID;
        }

        return cardList.GetCardStats(index);
    }

    public virtual void SetTripod(int index)
    {
        this.tripodNum = index;
        chainStats.ApplyOption(index);
    }

    public void TileSelect()
    {
        skillManager.hexTileSelectHandler.StartSelection(this.chainStats);
    }

    public virtual HexTile GetTargetTile(HexTile tile)
    {
        return tile;
    }

    public virtual List<HexTile> GetDamageTiles(List<HexTile> tiles)
    {
        return tiles;
    }


    public float GetDamage() => chainStats.skill_damage;
    public float GetIdentity() => chainStats.identityGain;
    public float GetStagger() => chainStats.stagger;

}
