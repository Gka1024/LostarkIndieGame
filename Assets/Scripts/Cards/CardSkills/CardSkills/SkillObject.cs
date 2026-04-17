using System.Collections;
using System.Collections.Generic;

public abstract class SkillObject
{
    protected bool isPlayAnimation = true;

    public abstract void ApplyOption(CardSkill card); // 스탯 조정

    public virtual IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        if (isBossHit) SkillManager.Instance.ApplyBossSkills(card.runtimeCardStats); // 스킬의 스탯의 데미지, 무력화, 파괴 처리

        card.manager.GetPlayer().GetComponent<PlayerStats>().UseMana(card.runtimeCardStats.manaUse); // 스킬의 스탯의 마나 처리

        if(isPlayAnimation)
        {
            SkillManager.Instance.PlayAnimaion(card, card.runtimeCardStats.playerWeapon, data.mainTile);
        }

        yield return null;
    } // 스킬 실행
}