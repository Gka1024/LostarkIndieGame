using UnityEngine;

[CreateAssetMenu(fileName = "OneHSword_HolyProtection", menuName = "CardSystem/OneHSword/HolyProtection")]
public class CardStats_OneHSword_HolyProtection : CardStats
{
    public float shield_amount;
    public int shield_turns;

    public float opt1_shield_bonus;
    public int opt1_shield_duration;

    public float opt2_heal;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                shield_amount += opt1_shield_bonus;
                shield_turns += opt1_shield_duration;
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