using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSkill_Hammer_PerfectSwing : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new Hammer_PerfectSwing_1(),
            2 => new Hammer_PerfectSwing_2(),
            3 => new Hammer_PerfectSwing_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }
}

public class Hammer_PerfectSwing_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class Hammer_PerfectSwing_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        yield return base.Execute(card);

        if (isBossHit)
        {
            if (card.runtimeCardStats is CardStats_Hammer_PerfectSwing stat)
            {
                SkillManager.Instance.MakeBossTaunt(stat.opt2_taunt_turns);
            }
        }
    }

}

public class Hammer_PerfectSwing_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}

/* 
    // ==== 트라이포드 1번
    public override void Option1()
    {
        CardStats_Hammer_PerfectSwing cardStat = gameObject.GetComponent<CardStats_Hammer_PerfectSwing>();
        baseCardStats.beforeActTurn += cardStat.opt1_turns;
        baseCardStats.skill_damage *= cardStat.opt1_damage_coef * 0.01f;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        if (IsPlayerNearbyBoss())
        {
            CardStats_Hammer_PerfectSwing cardStat = baseCardStats as CardStats_Hammer_PerfectSwing;
            bossStats.AddDebuff(new Debuff(DebuffType.Taunt, 1, cardStat.opt1_turns));
        }
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        CardStats_Hammer_PerfectSwing cardStat = gameObject.GetComponent<CardStats_Hammer_PerfectSwing>();
        baseCardStats.beforeActTurn -= 1;
        baseCardStats.skill_damage = cardStat.opt3_skill_damage_1;
        baseCardStats.isChainSkill = true;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }



    public override void SkillAnimation(HexTile tile)
    {
    }

    private bool IsPlayerNearbyBoss()
    {
        PlayerMove player = manager.GetPlayer().GetComponent<PlayerMove>();
        HexTile[] hexTiles = player.GetCurrentTile().neighbors;

        foreach (HexTile tile in hexTiles)
        {
            if (manager.hexTileManager.IsBossTile(tile))
            {
                return true;
            }
        }

        return false;
    }
*/