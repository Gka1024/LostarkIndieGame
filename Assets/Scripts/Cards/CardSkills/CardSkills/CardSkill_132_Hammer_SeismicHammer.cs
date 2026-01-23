using UnityEngine;

public class CardSkill_Hammer_SeismicHammer : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new Hammer_SeismicHammer_1(),
            2 => new Hammer_SeismicHammer_2(),
            3 => new Hammer_SeismicHammer_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }
}

public class Hammer_SeismicHammer_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class Hammer_SeismicHammer_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}


public class Hammer_SeismicHammer_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}





/*     // ==== 트라이포드 1번
    public override void Option1()
    {
        CardStats_Hammer_SeismicHammer cardStat = baseCardStats as CardStats_Hammer_SeismicHammer;
        baseCardStats.skill_damage += cardStat.opt1_damage_coef * 0.01f;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        baseCardStats.beforeActTurn--;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        baseCardStats.isChainSkill = true;
        ResetSkillType();
        baseCardStats.isAroundSkill = true;
        baseCardStats.aroundRange = 1;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }

    public override void SkillAnimation(HexTile tile)
    {

    }*/
