using UnityEngine;

public class CardSkill_OneHSword_HolySword : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new OneHSword_HolySword_1(),
            2 => new OneHSword_HolySword_2(),
            3 => new OneHSword_HolySword_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class OneHSword_HolySword_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class OneHSword_HolySword_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class OneHSword_HolySword_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}

/*     // ==== 트라이포드 1번
    public override void Option1()
    {
        CardStats_OneHSword_HolySword cardStat = gameObject.GetComponent<CardStats_OneHSword_HolySword>();
        ResetSkillType();

        cardStat.isRaySkill = true;
        cardStat.rayDistance = 3;
        cardStat.rayWidth = 1;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 2번
    public override void Option2()
    {

    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        CardStats_OneHSword_HolySword cardStat = gameObject.GetComponent<CardStats_OneHSword_HolySword>();
        cardStat.base_skill_damage_2 *= cardStat.opt3_damage_coef * 0.01f + 1;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }



    public override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
    }*/