using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id;
    public string name;
    public string description;
    public List<CardOption> options;
}

[System.Serializable]
public class CardOption
{
    public int option_id;
    public string option_name;
    public string option_description;
}

[System.Serializable]
public class CardDataList
{
    public List<CardData> cards;
}
