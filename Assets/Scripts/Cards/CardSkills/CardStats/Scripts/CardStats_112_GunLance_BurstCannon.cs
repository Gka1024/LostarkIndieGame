using UnityEngine;

[CreateAssetMenu(fileName = "GunLance_BurstCannon", menuName = "CardSystem/GunLance/BurstCannon")]
public class CardStats_GunLance_BurstCannon : CardStats
{
    public bool opt1_isChain;

    public float opt1_damage;
    public float opt1_stagger;
    public float opt1_identity;

    public float opt3_shield;
    public float opt3_damage;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                opt1_isChain = true;
                this.HasChainSkill = true;
                ChainSkill = CardList.Instance.GetChainSkills(this.CardID, 1);
                break;

            case 2:
                this.skillAngleRange++;
                break;

            case 3:
                this.skill_damage += opt3_damage;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }

    public SkillObject GetChainSkill()
    {
        return null;
    }
}
