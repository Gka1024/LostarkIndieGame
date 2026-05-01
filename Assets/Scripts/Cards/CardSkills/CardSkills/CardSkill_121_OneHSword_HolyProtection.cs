using System.Collections;
using UnityEngine;

public class CardSkill_OneHSword_HolyProtection : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new OneHSword_HolyProtection_1(),
            2 => new OneHSword_HolyProtection_2(),
            3 => new OneHSword_HolyProtection_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class OneHSword_HolyProtection_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        if (card.runtimeCardStats is CardStats_OneHSword_HolyProtection stat)
        {
            GameManager.Instance.GetPlayer().GetComponent<Player>().stats.AddShield(stat.shield_amount, stat.shield_turns);
        }
    }
}

public class OneHSword_HolyProtection_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        if (card.runtimeCardStats is CardStats_OneHSword_HolyProtection stat)
        {
            PlayerStats playerStats = GameManager.Instance.GetPlayer().GetComponent<Player>().stats;
            playerStats.AddShield(stat.shield_amount, stat.shield_turns, () => playerStats.Heal(stat.opt2_heal, true));
        }
    }
}

public class OneHSword_HolyProtection_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        if (card.runtimeCardStats is CardStats_OneHSword_HolyProtection stat)
        {
            PlayerStats playerStats = Player.Instance.stats;
            playerStats.AddShield(stat.shield_amount, stat.shield_turns, () => playerStats.buffState.RemoveBuff(BuffID_Player.PLAYER_SUPER_ARMOR));
            playerStats.buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_SUPER_ARMOR, stat.shield_turns));
        }
    }
}

/*  // ==== 트라이포드 1번
    public override void Option1()
    {
        CardStats_OneHSword_HolyProtection cardStat = gameObject.GetComponent<CardStats_OneHSword_HolyProtection>();
        cardStat.shield += cardStat.opt1_shield_bonus;
        cardStat.shield_turns += cardStat.opt1_shield_duration;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        CardStats_OneHSword_HolyProtection cardStat = gameObject.GetComponent<CardStats_OneHSword_HolyProtection>();
        playerStats.AddShield(cardStat.shield, cardStat.shield_turns);
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        return;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        CardStats_OneHSword_HolyProtection cardStat = gameObject.GetComponent<CardStats_OneHSword_HolyProtection>();
        playerStats.AddShield(cardStat.shield, cardStat.shield_turns, () => playerStats.Heal(cardStat.opt2_heal));
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        gameObject.GetComponent<CardStats_OneHSword_HolyProtection>().isSideSkill = true;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {
        CardStats_OneHSword_HolyProtection cardStat = gameObject.GetComponent<CardStats_OneHSword_HolyProtection>();
        playerStats.AddShield(cardStat.shield, cardStat.shield_turns);
    }

    public override void SkillAnimation(HexTile tile)
    {
        if (tile == null) // 즉시 시전인경우
        {

        }
    } */