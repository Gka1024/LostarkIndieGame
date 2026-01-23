using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChainStats", menuName = "CardSystem/ChainStats/ChainStats")]
public class ChainStats : ScriptableObject
{ // 인스펙터 창에서 기입
    public int afterDelay;

    public float skill_damage;
    public float stagger;
    public float identityGain;

    public bool isCounterAble;

    public List<HexTile> damageRange = new();
    public HexTile targetTile;

    public bool needToSelectTile;

    public bool isAngleSkill;
    public int skillAngle;
    public int skillAngleRange;

    public bool isDistanceSkill;
    public int skillDistance;
    public int skillDistanceRange;

    public bool isAroundSkill;
    public int aroundRange;

    public bool isRaySkill;
    public bool isHexRaySkill;
    public int rayDistance;
    public int rayWidth;

    public virtual void ApplyOption(int index){}
   
}
