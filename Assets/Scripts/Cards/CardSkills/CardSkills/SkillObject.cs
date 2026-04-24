using System.Collections;
using System.Collections.Generic;

public abstract class SkillObject
{
    protected bool isPlayAnimation = true;

    public abstract void ApplyOption(CardSkill card); // 스탯 조정

    public virtual IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        CardStats cardStats = card.runtimeCardStats;

        if (isBossHit) ApplyBossSkills(cardStats); // 스킬의 스탯의 데미지, 무력화, 파괴 처리

        UseMana(cardStats.manaUse); // 스킬의 스탯의 마나 처리

        if (isPlayAnimation) PlayAnimaion(card, data.mainTile); // 애니메이션 처리

        yield return null;
    }

    private void ApplyBossSkills(CardStats stat)
    {
        SkillManager.Instance.ApplyBossSkills(stat);
    }

    private void UseMana(float mana)
    {
        Player.Instance.stats.UseMana(mana);
    }

    private void PlayAnimaion(CardSkill card, HexTile tile)
    {
        SkillManager.Instance.PlayAnimaion(card, card.runtimeCardStats.playerWeapon, tile);
    }
}