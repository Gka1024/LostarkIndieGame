using System.Collections;
using System.Collections.Generic;

public abstract class SkillObject
{
    protected bool isBossHit;

    protected List<HexTile> selectedTiles;
    protected HexTile targetTile;

    public abstract void ApplyOption(CardSkill card); // 스탯 조정

    public virtual IEnumerator Execute(CardSkill card)
    {
        yield return null;
    } // 스킬 실행
}