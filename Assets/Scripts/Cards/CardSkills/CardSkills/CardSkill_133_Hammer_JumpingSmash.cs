using UnityEngine;

public class CardSkill_Hammer_JumpingSmash : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new Hammer_JumpingSmash_option1(),
            2 => new Hammer_JumpingSmash_option2(),
            3 => new Hammer_JumpingSmash_option3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        GameManager.Instance.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class Hammer_JumpingSmash_option1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class Hammer_JumpingSmash_option2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class Hammer_JumpingSmash_option3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}

/* // ==== 트라이포드 1번
    public override void Option1()
    {
        baseCardStats.beforeActTurn -= 1;
        ResetSkillType();
        baseCardStats.isRaySkill = true;
        baseCardStats.isHexRaySkill = true;
        baseCardStats.rayDistance = 2;
        baseCardStats.rayWidth = 1;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().MoveToTile(tile);
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        baseCardStats.skillDistance += 1;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().MoveToTile(tile);
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        baseCardStats.isChainSkill = true;
        baseCardStats.isDistanceSkill = false;
        baseCardStats.isAroundSkill = true;
        baseCardStats.aroundRange = 1;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }

    public override void SkillAnimation(HexTile tile)
    {

    }*/