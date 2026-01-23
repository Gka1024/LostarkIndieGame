using System.Collections;
using UnityEngine;

public class CardSkill_GunLance_ShoutOfHatred : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new GunLance_ShoutOfHatred1(),
            2 => new GunLance_ShoutOfHatred2(),
            3 => new GunLance_ShoutOfHatred3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class GunLance_ShoutOfHatred1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        Debug.Log("ApplyOption - GunLance_ShoutOfHatred1");
        card.runtimeCardStats.ApplyOption(1);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        yield return base.Execute(card);

        if (card.runtimeCardStats is CardStats_GunLance_ShoutOfHatred stat)
        {
            if (isBossHit)
            {
                SkillManager.Instance.MakeBossTaunt(stat.taunt_turns);
            }
        }
    }
}

public class GunLance_ShoutOfHatred2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        Debug.Log("ApplyOption - GunLance_ShoutOfHatred1");
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        yield return base.Execute(card);

        if (card.runtimeCardStats is CardStats_GunLance_ShoutOfHatred stat)
        {
            GameManager.Instance.GetPlayer().GetComponent<Player>().stats.AddShield(stat.opt2_shield, stat.opt2_shield_turns);
            if (isBossHit)
            {
                SkillManager.Instance.MakeBossTaunt(stat.taunt_turns);
            }
        }

    }
}

public class GunLance_ShoutOfHatred3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        Debug.Log("ApplyOption - GunLance_ShoutOfHatred1");
        card.runtimeCardStats.ApplyOption(3);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        yield return base.Execute(card);

        if (card.runtimeCardStats is CardStats_GunLance_ShoutOfHatred stat)
        {
            if (isBossHit)
            {
                SkillManager.Instance.MakeBossTaunt(stat.opt1_taunt_turns);
            }
        }
    }
}
/* // ==== 트라이포드 1번 
    public override void Option1()
    {
        baseCardStats.aroundRange++;
    }
    public override void Skill1(bool isBossHit, HexTile tile)
    {
        if (isBossHit)
        {
            AddIdentity(baseCardStats.identity);
        }
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        return;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        if (isBossHit)
        {
            AddIdentity(baseCardStats.identity);
        }

        CardStats_GunLance_ShoutOfHatred cardStat = gameObject.GetComponent<CardStats_GunLance_ShoutOfHatred>();

        playerStats.AddShield(cardStat.opt2_shield, cardStat.opt2_shield_turns);
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        return;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {
        if (isBossHit)
        {
            AddIdentity(baseCardStats.identity);
        }
    }

    public override void SkillAnimation(HexTile tile)
    {

    }*/
