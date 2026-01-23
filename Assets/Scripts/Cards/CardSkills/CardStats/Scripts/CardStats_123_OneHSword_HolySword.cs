using UnityEngine;

[CreateAssetMenu(fileName = "OneHSword_HolySword", menuName = "CardSystem/OneHSword/HolySword")]
public class CardStats_OneHSword_HolySword : CardStats
{
    public float chain_stagger;
    public float chain_identity;

    public float base_skill_damage_2;
    public int base_skill_range;

    public float opt2_damage;
    public float opt2_turns;

    public float opt3_damage_coef;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
