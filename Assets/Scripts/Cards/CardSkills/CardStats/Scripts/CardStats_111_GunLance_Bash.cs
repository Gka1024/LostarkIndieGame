using UnityEngine;

[CreateAssetMenu(fileName = "GunLance_Bash", menuName = "CardSystem/GunLance/Bash")]
public class CardStats_GunLance_Bash : CardStats
{
    [Header("옵션 1: 방어력 디버프")]
    public float opt1_defenceDebuff;
    public int opt1_turns;

    [Header("옵션 2: 스태거 보너스")]
    public float opt2_staggerBonus;

    [Header("옵션 3: 정체성 보너스")]
    public float opt3_identityBonus;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                // 방어력 감소 디버프 → SkillManager나 StatusEffectManager에서 처리
                Debug.Log($"{cardName} - 옵션1 선택: {opt1_defenceDebuff} 방깎, {opt1_turns}턴 지속");
                break;

            case 2:
                // 스태거 보너스 적용
                this.stagger += opt2_staggerBonus;
                Debug.Log($"{cardName} - 옵션2 선택: 스태거 {opt2_staggerBonus} 증가");
                break;

            case 3:
                // 정체성 게이지 추가 획득
                this.identityGain += opt3_identityBonus;
                Debug.Log($"{cardName} - 옵션3 선택: 정체성 {opt3_identityBonus} 추가");
                break;

            default:
                Debug.LogWarning($"ApplyOption: 알 수 없는 옵션 번호 {num}");
                break;
        }
    }
}
