using System.Collections.Generic;
using UnityEngine;

public class BattleItemDataBase : MonoBehaviour
{
    // 어디서든 접근 가능하도록 싱글톤 설정
    public static BattleItemDataBase Instance;

    [Header("JSON Data Source")]
    public TextAsset jsonText; // 유니티 인스펙터에서 JSON 파일을 할당하세요.

    // 전체 아이템 리스트 (인스펙터 확인용)
    [SerializeField] private List<ItemJSON> items = new();
    // ID를 통해 즉시 데이터를 찾기 위한 딕셔너리 (검색 최적화)
    private Dictionary<int, ItemJSON> itemDictionary = new Dictionary<int, ItemJSON>();

    [SerializeField] private List<BattleItemData> battleItemDatas = new();
    private Dictionary<int, BattleItemData> battleItemDictionary = new();

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            // 씬이 바뀌어도 데이터베이스가 파괴되지 않게 하려면 아래 주석 해제
            // DontDestroyOnLoad(gameObject); 
            LoadItemsFromJson();
            LoadBattleItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// JSON 텍스트를 파싱하여 리스트와 딕셔너리에 담습니다.
    /// </summary>
    private void LoadItemsFromJson()
    {
        if (jsonText == null)
        {
            Debug.LogError("BattleItemDataBase: JSON 파일이 할당되지 않았습니다!");
            return;
        }

        try
        {
            // JSON 파싱
            ItemJSONList itemList = JsonUtility.FromJson<ItemJSONList>(jsonText.text);
            items = itemList.Items;

            // 딕셔너리 생성 (검색 성능 향상: O(1))
            itemDictionary.Clear();
            foreach (var item in items)
            {
                if (!itemDictionary.ContainsKey(item.ID))
                {
                    itemDictionary.Add(item.ID, item);
                }
                else
                {
                    Debug.LogWarning($"BattleItemDataBase: 중복된 ID가 발견되었습니다: {item.ID}");
                }
            }

            Debug.Log($"BattleItemDataBase: 총 {itemDictionary.Count}개의 아이템 JSON 로드 완료.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"BattleItemDataBase: JSON 로드 중 오류 발생! {e.Message}");
        }
    }

    /// <summary>
    /// ID를 사용하여 아이템의 이름과 설명 데이터를 가져옵니다.
    /// </summary>
    public ItemJSON GetItemJSONByID(int id)
    {
        if (itemDictionary.TryGetValue(id, out ItemJSON data))
        {
            return data;
        }

        Debug.LogWarning($"BattleItemDataBase: ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
        return null;
    }

    private void LoadBattleItems()
    {
        foreach (var data in battleItemDatas)
        {
            battleItemDictionary.Add(data.itemID, data);
        }

        Debug.Log($"BattleItemDataBase : 총 {battleItemDictionary.Count} 개의 아이템 로드 완료.");
    }

    public BattleItemData GetItemDataByID(int id)
    {
        if (battleItemDictionary.TryGetValue(id, out BattleItemData data))
        {
            return data;
        }

        Debug.LogWarning($"BattleItemDataBase: ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
        return null;
    }
}

// --- JSON 파싱을 위한 직렬화 클래스들 ---

[System.Serializable]
public class ItemJSONList
{
    public List<ItemJSON> Items;
}

[System.Serializable]
public class ItemJSON
{
    public int ID;
    public string Name;
    public string Description;
}