using System.Collections;
using UnityEngine;

public class CardSkill_TwoHSword_VolcanoEruption : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new TwoHSword_VolcanoEruption_1(),
            2 => new TwoHSword_VolcanoEruption_2(),
            3 => new TwoHSword_VolcanoEruption_3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class TwoHSword_VolcanoEruption_1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        Player.Instance.move.MoveToTile(new PlayerMoveInfo(data.mainTile, false, true, true, false));
    }
}

public class TwoHSword_VolcanoEruption_2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        Player.Instance.move.MoveToTile(new PlayerMoveInfo(data.mainTile, false, true, true, false));
    }
}

public class TwoHSword_VolcanoEruption_3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        yield return base.Execute(card, data, isBossHit);

        Player.Instance.move.MoveToTile(new PlayerMoveInfo(data.mainTile, false, true, true, false));
        Player.Instance.stats.buffState.AddBuff(PlayerBuffFactory.CreateBuff(BuffID_Player.PLAYER_SUPER_ARMOR, card.runtimeCardStats.beforeActTurn + card.runtimeCardStats.afterActTurn + 1));

    }

}

/* // ==== 트라이포드 1번
    public override void Option1()
    {
        baseCardStats.skillDistance += 1;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().MoveToTile(tile, true, true);
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        CardStats_TwoHSword_VolcanoEruption cardStat = baseCardStats as CardStats_TwoHSword_VolcanoEruption;
        baseCardStats.skill_damage *= 1 + (cardStat.opt2_damage_coef * 0.01f);
        baseCardStats.identity = 0;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {

    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {

    }


    public override void SkillAnimation(HexTile tile)
    {

    }*/