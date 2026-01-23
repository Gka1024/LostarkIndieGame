using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSkill_GunLance_Bash : CardSkill
{
    public bool applyDebuffOnHit = false;

    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new GunLance_Bash_Option1(),
            2 => new GunLance_Bash_Option2(),
            3 => new GunLance_Bash_Option3(),
            _ => null
        };
    }

    public override void ApplySkill(bool isBossInRange = false, HexTile tile = null)
    {
        base.ApplySkill(isBossInRange, tile);
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class GunLance_Bash_Option1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        if (card is CardSkill_GunLance_Bash cardSkill_GunLance_Bash)
        {
            cardSkill_GunLance_Bash.runtimeCardStats.ApplyOption(1);
            cardSkill_GunLance_Bash.applyDebuffOnHit = true;
        }
    }

    public override IEnumerator Execute(CardSkill card)
    {
        base.Execute(card);

        if (isBossHit)
        {
            var cardStat = card.GetStats<CardStats_GunLance_Bash>();
            card.manager.GetBoss().GetComponent<BossStatus>().AddBossDebuff(new BossDebuff(DebuffType.DefenceDown, cardStat.opt1_defenceDebuff, cardStat.opt1_turns, 1));
        }

        yield return null;
    }
}

public class GunLance_Bash_Option2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class GunLance_Bash_Option3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}


/*
 protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.ChangeWeapon(PlayerWeapon.Gunlance);
        playerAnimation.PlayAnimation(1);
    }
    // ==== 트라이포드 1번
    public override void Option1()
    {
        return;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        CardStats_GunLance_Bash cardStat = gameObject.GetComponent<CardStats_GunLance_Bash>();
        if (isBossHit)
        {
            bossStats.AddDebuff(new Debuff(DebuffType.DefenceDown, cardStat.opt1_defenceDebuff, cardStat.opt1_turns));
        }
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        cardStats.stagger += this.gameObject.GetComponent<CardStats_GunLance_Bash>().opt2_staggerBonus;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        float changeIdentityRatio = gameObject.GetComponent<CardStats_GunLance_Bash>().opt3_identityBonus * 0.01f;
        cardStats.identity *= changeIdentityRatio;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }
    */