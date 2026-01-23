using UnityEngine;

public class CardSkill_TwoHSword_FinishStrike : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new TwoHSword_FinishStrike_1(),
            2 => new TwoHSword_FinishStrike_2(),
            3 => new TwoHSword_FinishStrike_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class TwoHSword_FinishStrike_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class TwoHSword_FinishStrike_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class TwoHSword_FinishStrike_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}

/*    // ==== 트라이포드 1번
    public override void Option1()
    {
        baseCardStats.beforeActTurn = 0;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        baseCardStats.isChainSkill = true;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        baseCardStats.isChainSkill = true;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }


    public override void SkillAnimation(HexTile tile)
    {
    }*/