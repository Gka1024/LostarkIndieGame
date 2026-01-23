using UnityEngine;

public class CardSkill_TwoHSword_RedDust : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new TwoHSword_RedDust_1(),
            2 => new TwoHSword_RedDust_2(),
            3 => new TwoHSword_RedDust_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class TwoHSword_RedDust_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class TwoHSword_RedDust_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class TwoHSword_RedDust_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}

/*  // ==== 트라이포드 1번
    public override void Option1()
    {
        float changeIdentityRatio = gameObject.GetComponent<CardStats_TwoHSword_RedDust>().opt1_identity_bonus * 0.01f;
        baseCardStats.identity *= changeIdentityRatio;
        return;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        CardStats_TwoHSword_RedDust cardStat = baseCardStats as CardStats_TwoHSword_RedDust;

        playerStats.AddAttackBuff(cardStat.base_buff_attack, 0, cardStat.base_buff_turns);
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        CardStats_TwoHSword_RedDust cardStat = baseCardStats as CardStats_TwoHSword_RedDust;

        cardStat.base_buff_attack += cardStat.opt2_attack_increase;
        cardStat.base_buff_turns += cardStat.opt2_increase_turns;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        CardStats_TwoHSword_RedDust cardStat = baseCardStats as CardStats_TwoHSword_RedDust;

        playerStats.AddAttackBuff(cardStat.base_buff_attack, 0, cardStat.base_buff_turns);
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {

    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {
        CardStats_TwoHSword_RedDust cardStat = baseCardStats as CardStats_TwoHSword_RedDust;

        playerStats.AddAttackBuff(cardStat.base_buff_attack, 0, cardStat.base_buff_turns);
    }


    public override void SkillAnimation(HexTile tile)
    {
    }
*/

