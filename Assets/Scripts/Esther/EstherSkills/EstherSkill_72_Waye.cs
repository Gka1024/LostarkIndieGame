using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Waye : EstherSkill
{
    public EstherSkill_Data_Waye skillData;

    public override void Init(HexTile spawnTile)
    {
        base.Init(spawnTile);
        EstherSkillTurnMax = skillData.EstherSkillTurnMax;
    }


    public override void Execute(HexTile targetTile, List<HexTile> selectedTiles)
    {
        throw new System.NotImplementedException();
    }
}
