using System.Collections.Generic;
using UnityEngine;

public class ChainSkillList : MonoBehaviour
{
    public List<GameObject> chainSkills = new();

    public Dictionary<int, GameObject> chainDictionary = new();


    public GameObject GetChainSkill(int index)
    {
        if (chainDictionary.TryGetValue(index, out GameObject chainSkill))
        {
            return chainSkill;
        }
        return null;
    }

}
