using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainSkill : MonoBehaviour
{
    public int CardID;
    public int CardTripodNum;

    public ChainStats chainStats;

    public CardSkill baseCardSkill;
    public int tripodNum;

    public virtual IEnumerator ExecuteChain(SkillQueueData data, bool isBossHit)
    {
        if (isBossHit) ApplyBossSkills(chainStats); // 스킬의 스탯의 데미지, 무력화, 파괴 처리

        PlayAnimation(data.mainTile); // 애니메이션 처리

        yield return null;
    }

    public virtual void SetTripod(int index)
    {
        this.tripodNum = index;
        chainStats.ApplyOption(index);
    }

    public float GetDamage() => chainStats.skill_damage;
    public float GetIdentity() => chainStats.identityGain;
    public float GetStagger() => chainStats.stagger;


    private void ApplyBossSkills(ChainStats stat)
    {
        SkillManager.Instance.ApplyBossSkills(stat);
    }

    private void UseMana(float mana)
    {
        Player.Instance.stats.UseMana(mana);
    }

    protected void PlayAnimation(HexTile tile)
    {
        Player.Instance.move.RotateToTile(tile);
        Player.Instance.anim.PlayAnimation(1);
    }
}
