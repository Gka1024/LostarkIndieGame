using UnityEngine;

[CreateAssetMenu(fileName = "New CardStats", menuName = "CardSystem/CardStats")]
public class CardStats : ScriptableObject
{ // 인스펙터 창에서 기입
    public int CardID;
    public string cardName;
    public bool isCardSkill; // 카드 스킬인지를 확인하는 변수입니다. 카드 스킬이 아닌 경우는 더미 데이터, 체인 스킬 등이 있습니다.

    public bool HasChainSkill;
    public GameObject ChainSkill;

    public PlayerWeapon playerWeapon;

    // ==== 딜레이 변수
    public int beforeActTurn;
    public int afterActTurn;

    // ==== 타일 선택 관련 변수
    public bool needToSelectTile;

    public TileSelectType tileSelectType;

    public int skillAngle;
    public int skillAngleRange;

    public int skillDistance;
    public int skillDistanceRange;

    public int aroundRange;

    public int rayDistance;
    public int rayWidth;

    // ==== 스킬 상세 변수
    public float skill_damage; // playerStats.GetPlayerAttack() * skill_damage 가 실제 데미지 입니다.
    public float manaUse; // 플레이어 최대 마나 250, 플레이어 턴마다 마나 회복 20;
    public float identityGain; // 플레이어 최대 아덴 200
    public float stagger; // 0~20 : 하 / 21~45 : 중 / 46+ 상
    public int coolDownTurn; // 스킬 쿨타임(턴으로 계산)

    // ==== 스킬 속성 관련 변수
    public bool isCounterAble;
    public bool isHeadAttack;
    public bool isBackAttack;
    public bool isSuperArmor= false;

    public virtual void ApplyOption(int num) { }

    public T Clone<T>() where T : CardStats
    {
        return Instantiate(this) as T;
    }
}
