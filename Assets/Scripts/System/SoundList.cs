using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    [System.Serializable]
    public struct SoundMapping
    {
        public int soundId;
        public AudioClip clip;
        [Range(0, 1)]
        public float defaultVolume;
    }

    // 여기에 mp3 파일들을 리스트로 등록하세요.
    public List<SoundMapping> sounds = new List<SoundMapping>();

    private void OnValidate()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            var sound = sounds[i];
            // 처음 추가되어 볼륨이 0인 경우에만 1로 자동 설정
            if (sound.defaultVolume <= 0 && sound.clip != null)
            {
                sound.defaultVolume = 1.0f;
                sounds[i] = sound;
            }
        }
    }
}

public class SoundID
{
    public const int MENU = 001;

    public const int ROAR = 101;
    public const int AIR_PUFF = 102;
    public const int CHANG = 103;
    public const int KWANG = 104;
    public const int KUNG = 105;
    public const int KONG = 106;
    public const int WIND = 107;
    public const int WIND_KWANG = 108;
    public const int KONG2 = 109;
    public const int PANG = 110;
    public const int CHANGRANG = 111;
    
}