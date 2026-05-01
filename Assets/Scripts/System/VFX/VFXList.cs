using System.Collections.Generic;
using UnityEngine;

public class VFXList : MonoBehaviour
{
    public VFXManager vfxManager;

    // 범위 설정 (예: Field=1000번대, Player=2000번대, Boss=3000번대)
    public int fieldOffset = 1001;
    public int playerOffset = 2001;
    public int bossOffset = 3001;
    public int itemOffset = 4001;
    public int estherOffset = 5001;

    public List<GameObject> VFXS_Field;
    public List<GameObject> VFXS_Player;
    public List<GameObject> VFXS_Boss;
    public List<GameObject> VFXS_BattleItem;
    public List<GameObject> VFXS_EstherSkill;

    private void Awake()
    {
        CheckVFXRegistered();
    }

    private void CheckVFXRegistered()
    {
        // 1. 등록된 총 개수를 저장할 변수
        int totalCount = 0;

        // 2. 각각 등록하고 개수 합산
        totalCount += RegisterList(VFXS_Field, fieldOffset);
        totalCount += RegisterList(VFXS_Player, playerOffset);
        totalCount += RegisterList(VFXS_Boss, bossOffset);
        totalCount += RegisterList(VFXS_BattleItem, itemOffset);
        totalCount += RegisterList(VFXS_EstherSkill, estherOffset);

        // 3. 마지막에 딱 한 번만 로그 출력
        Debug.Log($"VFXList: 총 {totalCount}개의 VFX 로드 완료.");
    }

    private int RegisterList(List<GameObject> list, int offset)
    {
        if (list == null) return 0; // null 체크 방지

        for (int i = 0; i < list.Count; i++)
        {
            int id = offset + i;
            vfxManager.RegisterVFX(id, list[i]);
        }

        return list.Count; // 등록한 개수 반환
    }
}

public static class VFXID
{
    public const int Field_Explosion_01 = 1001;
    public const int Field_Explosion_02 = 1002;
    public const int Field_Flame = 1003;
    public const int Field_Impact = 1004;
    public const int Field_LootDrop_01 = 1005;
    public const int Field_LootDrop_02 = 1006;
    public const int Field_ShockWave = 1007;
    public const int Field_Smoke = 1008;
    public const int Field_Ground_Explosion = 1009;
    public const int Field_Smoke_Explosion = 1010;
    public const int Field_Lightning = 1011;
    public const int Field_Explosion_03 = 1012;
    public const int Field_Holy_Hit = 1013;
    public const int Field_Heal_Circle = 1014;

    public const int Player_Heal = 2001;
    public const int Player_Buff = 2002;
    public const int Player_Shield = 2003;
    public const int Player_Smoke = 2004;
    public const int Player_Gold = 2005;
    public const int Player_Identity = 2006;

    public const int Boss_Shield = 3001;
    public const int Boss_Shield_Circle = 3002;
    public const int Boss_Electric = 3003;
    public const int Boss_Implosion = 3004;

    public const int BattleItem_Area_Heal = 4001;
    public const int BattleItem_Granade_Clay = 4002;
    public const int BattleItem_Granade_Corrosion = 4003;
    public const int BattleItem_Granade_Dark = 4004;
    public const int BattleItem_Granade_Destruction = 4005;
    public const int BattleItem_Granade_Lightning = 4006;
    public const int BattleItem_Granade_Flaiming = 4007;
    public const int BattleItem_Granade_Flasing = 4008;
    public const int BattleItem_Granade_Tornado = 4009;

    public const int Esther_Bahuntur = 5001;
    public const int Esther_Thirain = 5002;
    public const int Esther_Thirain_Projectile = 5003;
    public const int Esther_Waye = 5004;


}