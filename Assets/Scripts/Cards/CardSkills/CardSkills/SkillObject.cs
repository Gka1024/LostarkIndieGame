using System.Collections;
using System.Collections.Generic;

public abstract class SkillObject
{
    protected List<HexTile> selectedTiles;
    protected HexTile targetTile;

    public abstract void ApplyOption(CardSkill card); // 스탯 조정

    public virtual IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        if (isBossHit) SkillManager.Instance.ApplyBossSkills(card.runtimeCardStats);
        card.manager.GetPlayer().GetComponent<PlayerStats>().UseMana(card.runtimeCardStats.manaUse);
        yield return null;
    } // 스킬 실행
}