using UnityEngine;

[CreateAssetMenu(fileName = "OneHSword_HeavenlyBlessing", menuName = "CardSystem/OneHSword/HeavenlyBlessing")]
public class CardStats_OneHSword_HeavenlyBlessing : CardStats
{
    public float base_buff_attack;
    public int base_buff_turns;

    public int opt1_turns;
    public float opt1_mana_regen;

    public float opt2_buff_attack;
    public int opt2_buff_turns;

    public float opt3_identity_bonus;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                base_buff_turns += opt1_turns;
                break;

            case 2:
                base_buff_attack += opt2_buff_attack;
                base_buff_turns += opt2_buff_turns;
                break;

            case 3:
                identityGain += opt3_identity_bonus;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
