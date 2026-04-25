using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [System.Serializable]
    public struct VFXMapping
    {
        public int effectId;
        public GameObject prefab;
    }

    [Header("Settings")]
    public List<VFXMapping> effectLibrary; // 인스펙터에서 ID와 프리팹 연결
    public Transform poolRoot; // 하이어라키 정리용 부모

    private Dictionary<int, GameObject> _prefabDict = new();
    private Dictionary<int, Queue<GameObject>> _poolDict = new();

    private void Awake()
    {
        Instance = this;
        foreach (var item in effectLibrary)
        {
            _prefabDict[item.effectId] = item.prefab;
            _poolDict[item.effectId] = new Queue<GameObject>();
        }
    }

    public void PlayEffect(int id, HexTile tile, float duration = -1f)
    {
        PlayEffect(id, tile.transform.position, duration);
    }

    public void PlayEffect(int id, List<HexTile> tiles, float duration = -1f)
    {
        foreach(HexTile tile in tiles)
        {
            PlayEffect(id, tile, duration);
        }
    }

    public void PlayEffectAtPlayer(int id, float duration = -1f)
    {
        PlayEffect(id, Player.Instance.move.GetCurrentTile(), duration);
    }

    public void PlayEffect(int id, Vector3 position, float duration = -1f)
    {
        if (!_prefabDict.TryGetValue(id, out GameObject prefab)) return;

        GameObject vfxObj = GetFromPool(id, prefab, position, duration);
    }

    private GameObject GetFromPool(int id, GameObject prefab, Vector3 pos, float duration)
    {
        GameObject obj;
        if (_poolDict[id].Count > 0)
        {
            obj = _poolDict[id].Dequeue();
            obj.transform.position = pos;
        }
        else
        {
            obj = Instantiate(prefab, pos, Quaternion.identity, poolRoot);
        }

        obj.SetActive(true);

        var autoReturn = obj.GetComponent<VFXAutoReturn>();
        if (autoReturn == null) autoReturn = obj.AddComponent<VFXAutoReturn>();

        autoReturn.Initialize(id, duration);

        return obj;
    }

    public void ReturnToPool(int id, GameObject obj)
    {
        obj.SetActive(false);
        _poolDict[id].Enqueue(obj);
    }

    public void RegisterVFX(int id, GameObject prefab)
    {
        if (_prefabDict.ContainsKey(id))
        {
            Debug.LogWarning($"이미 등록된 ID입니다: {id}. 덮어씁니다.");
        }

        _prefabDict[id] = prefab;
        // 풀 초기화
        if (!_poolDict.ContainsKey(id))
        {
            _poolDict[id] = new Queue<GameObject>();
        }
    }



    public void MakeVFXForDebug()
    {
        PlayEffectAtPlayer(VFXID.Player_Heal, 1);
    }
}
