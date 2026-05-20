using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChainStats", menuName = "CardSystem/ChainStats/ChainStats")]
public class ChainStats : ScriptableObject
{ // 인스펙터 창에서 기입
    public int cardID;
    public int tripodIndex;

    public GameObject chainSkillPrefab;

    public int afterDelay;

    public float skill_damage;
    public float stagger;
    public float identityGain;

    public bool isCounterAble;

    public List<HexTile> damageRange = new();
    public HexTile targetTile;

    public bool needToSelectTile;
    public TileSelectType tileSelectType;

    public int skillAngle;
    public int skillAngleRange;

    public int skillDistance;
    public int skillDistanceRange;

    public int aroundRange;

    public int rayDistance;
    public int rayWidth;

    public virtual void ApplyOption(int index) { }

    public virtual void SetDamage(float damage)
    {
        skill_damage = damage;
    }

    public virtual void MultiflyDamage(float damageOverride)
    {
        skill_damage *= 1 + damageOverride * 0.01f;
    }
}
