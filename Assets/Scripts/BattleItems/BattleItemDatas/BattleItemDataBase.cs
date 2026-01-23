using System.Collections.Generic;
using UnityEngine;

public class BattleItemDataBase : MonoBehaviour
{
    public static BattleItemDataBase Instance;
    public TextAsset jsonText;

    public List<ItemData> items = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        LoadItemsFromJson();
    }

    private void LoadItemsFromJson()
    {
        ItemDataList itemList = JsonUtility.FromJson<ItemDataList>(jsonText.text);
        items = itemList.Items;
    }

    public ItemData GetItemDataByID(int id)
    {
        return items.Find(item => item.ID == id);
    }
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemData> Items;
}

[System.Serializable]
public class ItemData
{
    public int ID;
    public string Name;
    public string Description;
}
