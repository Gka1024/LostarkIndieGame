using UnityEngine;

public class CardSkill_OneHSword_HeavenlyBlessing : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new OneHSword_HeavenlyBlessing_1(),
            2 => new OneHSword_HeavenlyBlessing_2(),
            3 => new OneHSword_HeavenlyBlessing_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class OneHSword_HeavenlyBlessing_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class OneHSword_HeavenlyBlessing_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class OneHSword_HeavenlyBlessing_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}


/*    // ==== 트라이포드 1번
    public override void Option1()
    {
        return;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        CardStats_OneHSword_HeavenlyBlessing cardStat = baseCardStats as CardStats_OneHSword_HeavenlyBlessing;

        playerStats.AddAttackBuff(cardStat.base_buff_attack, 0, cardStat.base_buff_turns);
        playerStats.AddManaRegenBuff(cardStat.opt1_mana_regen, cardStat.opt1_turns);
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        CardStats_OneHSword_HeavenlyBlessing cardStat = baseCardStats as CardStats_OneHSword_HeavenlyBlessing;

        cardStat.base_buff_attack += cardStat.opt2_buff_attack;
        cardStat.base_buff_turns += cardStat.opt2_buff_turns;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        CardStats_OneHSword_HeavenlyBlessing cardStat = baseCardStats as CardStats_OneHSword_HeavenlyBlessing;

        playerStats.AddAttackBuff(cardStat.base_buff_attack, 0, cardStat.base_buff_turns);
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        CardStats_OneHSword_HeavenlyBlessing cardStat = gameObject.GetComponent<CardStats_OneHSword_HeavenlyBlessing>();
        cardStat.identity += cardStat.opt3_identity_bonus;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {
    }

    public override void SkillAnimation(HexTile tile = null)
    {
    }
 */