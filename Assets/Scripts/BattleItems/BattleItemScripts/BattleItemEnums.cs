public enum ItemType
{
    Unset,
    Potion,
    Granade,
    Special
}

public enum PotionType
{
    Unset,
    Heal, // 플레이어의 체력 회복
    Identity, // 플레이어의 아이덴티티를 즉시 채울 수 있음
    Atropine, // 최대 체력의 20%를 사용하지만 가하는 데미지 증가
    Shield, // 최대 체력의 50%만큼의 보호막을 즉시 제공
    TimeStop // 일부 기믹을 제외하고, 데미지를 받지 않음. 
}

public enum GranadeType
{
    Unset,
    Clay, // 공격력 감소
    Corrosion, // 파괴량 증가
    Dark, // 방어력 감소
    Destruction, // 파괴 부여
    Electric, // 데미지 및 쉴드 감소
    Flaming, // 지속 데미지
    Flashing, // 기절
    Tornado // 무력화
}

public enum SpecialType
{
    Unset,
    ScareCrow, // 몇 턴 동안 보스가 허수아비를 우선 공격함
    PaperAmulet, // 플레이어의 부정적인 효과 제거
    HidingRobe, // 보스가 플레이어를 찾을 수 없음. 
    CampFire, // 주변에 플레이어가 있다면 지속적으로 힐을 함
    MarchingFlag // 플레이어의 스페 초기화
}