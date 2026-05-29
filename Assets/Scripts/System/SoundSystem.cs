using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // 믹서 API 사용

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Resources")]
    public SoundList soundListContainer; // 하이어라키의 SoundList 연결

    [Header("Audio Mixer & Groups")]
    public AudioMixer mainMixer;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    [Header("BGM")]
    public AudioSource bgmSource;

    private Dictionary<int, SoundList.SoundMapping> _soundDict = new();
    private Queue<AudioSource> _pool = new();
    private Dictionary<int, float> _lastPlayTimes = new();

    private const float MIN_REPLAY_DELAY = 0.05f; // 동일 사운드 중복 방지 시간

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeManager();
    }

    private void InitializeManager()
    {
        // 1. 사운드 리스트 등록 (Register)
        if (soundListContainer != null)
        {
            foreach (var item in soundListContainer.sounds)
            {
                if (item.clip == null) continue;
                _soundDict[item.soundId] = item;
            }
        }

        // 2. BGM 소스 설정
        if (bgmSource != null) bgmSource.outputAudioMixerGroup = bgmGroup;

        // 3. SFX 풀 미리 생성 (초기 10개)
        for (int i = 0; i < 10; i++)
        {
            _pool.Enqueue(CreateNewSource());
        }

        Debug.Log($"SoundManager : {_soundDict.Count}개의 사운드 등록 완료");
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = new GameObject("SFX_Source");
        go.transform.SetParent(transform);
        AudioSource source = go.AddComponent<AudioSource>();

        // 믹서 그룹 연결 (중요!)
        source.outputAudioMixerGroup = sfxGroup;
        source.playOnAwake = false;

        go.SetActive(false);
        return source;
    }

    /// <summary>
    /// 효과음 재생 (ID 1번부터 시작 권장)
    /// </summary>
    public void PlaySFX(int id, Vector3? position = null, float volumeMult = 1f, bool adjustPitch = false)
    {
        if (id <= 0 || !_soundDict.TryGetValue(id, out var data)) return;

        // 중복 재생 방지 로직
        if (_lastPlayTimes.TryGetValue(id, out float lastTime))
        {
            if (Time.time - lastTime < MIN_REPLAY_DELAY) return;
        }
        _lastPlayTimes[id] = Time.time;

        AudioSource source = GetSourceFromPool();

        // 3D/2D 설정
        if (position.HasValue)
        {
            source.transform.position = position.Value;
            source.spatialBlend = 1.0f; // 3D
            source.minDistance = 2f;
            source.maxDistance = 20f;
        }
        else
        {
            source.spatialBlend = 0.0f; // 2D
        }

        // 풍성한 사운드를 위한 피치 랜덤화 (0.95 ~ 1.05)
        if (adjustPitch) source.pitch = Random.Range(0.95f, 1.05f);
        source.clip = data.clip;
        float baseVolume = data.defaultVolume <= 0 ? 1.0f : data.defaultVolume;
        source.volume = baseVolume * volumeMult;

        source.gameObject.SetActive(true);
        source.Play();

        StartCoroutine(ReturnToPoolAfterPlay(source, data.clip.length));
    }

    private AudioSource GetSourceFromPool()
    {
        if (_pool.Count > 0)
        {
            AudioSource source = _pool.Dequeue();
            return source;
        }
        return CreateNewSource();
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource source, float duration)
    {
        // 피치가 달라지면 재생 시간도 미세하게 달라짐을 반영
        yield return new WaitForSeconds(duration / source.pitch);

        source.Stop();
        source.gameObject.SetActive(false);
        _pool.Enqueue(source);
    }

    // 믹서 볼륨 조절 API (설정창용)
    public void SetGroupVolume(string parameterName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, sliderValue)) * 20f;
        mainMixer.SetFloat(parameterName, dB);
    }

    public void PlaySoundDebug()
    {
        PlaySFX(3);
    }
}