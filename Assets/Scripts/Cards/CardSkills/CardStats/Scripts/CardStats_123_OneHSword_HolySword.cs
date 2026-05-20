using UnityEngine;

[CreateAssetMenu(fileName = "OneHSword_HolySword", menuName = "CardSystem/OneHSword/HolySword")]
public class CardStats_OneHSword_HolySword : CardStats
{
    public float chain_stagger;
    public float chain_identity;

    public float base_skill_damage_2;
    public int base_skill_range;

    public float opt2_damage;
    public int opt2_turns;

    public float opt3_damage_coef;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                this.rayDistance += 3;
                break;

            case 2:
                break;

            case 3:
                this.rayDistance--;
                this.skill_damage *= 1 + opt3_damage_coef * 0.01f;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }

    public override ChainStats GetChainStats(int tripodIndex)
    {
        ChainStats original = chainPaths.Find(p => p.tripodIndex == tripodIndex)?.chainStats;

        if (original == null) return null;

        ChainStats clonedStats = Instantiate(original);
        clonedStats.SetDamage(base_skill_damage_2);

        if (tripodIndex == 3)
        {
            clonedStats.MultiflyDamage(opt3_damage_coef);
        }

        return clonedStats;
    }
}
