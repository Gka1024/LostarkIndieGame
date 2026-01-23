using UnityEngine;

[CreateAssetMenu(fileName = "GunLance_ShoutOfHatred", menuName = "CardSystem/GunLance/ShoutOfHatred")]
public class CardStats_GunLance_ShoutOfHatred : CardStats
{
    public int taunt_turns;

    public int opt1_taunt_turns;

    public int opt2_shield_turns;
    public float opt2_shield;

    public float opt3_identity_bonus;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                this.taunt_turns += opt1_taunt_turns;
                break;

            case 2:
                break;

            case 3:
                this.identityGain += opt3_identity_bonus;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
