using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Vector3 tileVFXOffset;

    [Header("Settings")]
    public List<VFXMapping> effectLibrary; // 인스펙터에서 ID와 프리팹 연결
    public Transform poolRoot; // 하이어라키 정리용 부모

    private Dictionary<int, GameObject> _prefabDict = new();
    private Dictionary<int, Queue<GameObject>> _poolDict = new();

    private List<VFXAutoReturn> _activeTurnVFXs = new();

    private void Awake()
    {
        Instance = this;
        foreach (var item in effectLibrary)
        {
            _prefabDict[item.effectId] = item.prefab;
            _poolDict[item.effectId] = new Queue<GameObject>();
        }

        tileVFXOffset = new Vector3(0, 2f, 0);
    }

    public void PlayEffect(int id, HexTile tile, float duration = -1f)
    { // 시간 단위 이펙트
        PlayEffect(id, tile.transform.position + tileVFXOffset, duration);
    }

    public void PlayEffect(int id, HexTile tile, int turnDuration, float yoffset = 0)
    { // 턴 단위 이펙트
        Vector3 offset = new Vector3(0, yoffset, 0);
        PlayEffect(id, tile.transform.position + tileVFXOffset + offset, 0, turnDuration);
    }

    public void PlayEffect(int id, List<HexTile> tiles, float duration = -1f)
    { // 여러 타일 이펙트
        StartCoroutine(PlayEffectSequence(id, tiles, duration));
    }

    private IEnumerator PlayEffectSequence(int id, List<HexTile> tiles, float duration)
    {
        // 1. (선택사항) 플레이어 위치 기준으로 거리 정렬 (가까운 곳부터 터지게)
        var playerPos = Player.Instance.transform.position;
        var sortedTiles = tiles.OrderBy(t => Vector3.Distance(t.transform.position, playerPos)).ToList();

        // 2. 시간 간격 설정 (0.05초~0.1초 정도가 적당합니다)
        float delay = 0.03f;

        foreach (HexTile tile in sortedTiles)
        {
            // 3. 이펙트 재생
            PlayEffect(id, tile.transform.position, duration);

            // 4. 아주 짧게 대기
            yield return new WaitForSeconds(delay);
        }
    }

    public void PlayEffectAtPlayer(int id, float duration = -1f)
    {
        PlayEffect(id, Player.Instance.move.GetCurrentTile(), duration);
    }

    public void PlayEffect(int id, Vector3 position, float duration = -1f, int turnDuration = 0)
    {
        if (!_prefabDict.TryGetValue(id, out GameObject prefab)) return;

        GameObject vfxObj = GetFromPool(id, prefab, position, duration, turnDuration);
    }

    private GameObject GetFromPool(int id, GameObject prefab, Vector3 pos, float duration, int turnDuration)
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

        autoReturn.Initialize(id, duration, turnDuration);
        if (turnDuration > 0) _activeTurnVFXs.Add(autoReturn);

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

    public void PlayProjectile(int id, HexTile startTile, HexTile targetTile, float speed, float lifetime)
    {
        Vector3 direction = targetTile.transform.position - startTile.transform.position;

        PlayProjectile(id, startTile.transform.position, direction, speed, lifetime);
    }

    public void PlayProjectile(int id, Vector3 startPos, Vector3 direction, float speed, float lifetime)
    {
        if (!_prefabDict.TryGetValue(id, out GameObject prefab)) return;

        GameObject obj = GetFromPool(id, prefab, startPos, lifetime, 0);

        // 프로젝타일 컴포넌트 설정
        var projectile = obj.GetComponent<VFXProjectile>();
        if (projectile == null) projectile = obj.AddComponent<VFXProjectile>();

        projectile.Initialize(direction, speed);
    }
}
