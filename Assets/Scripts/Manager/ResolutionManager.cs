using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요합니다.

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        // 1. 현재 모니터가 지원하는 모든 해상도를 가져옵니다.
        Resolution[] allResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            // 부드러운 화면을 위해 현재 모니터의 주사율(Refresh Rate)과 일치하는 해상도만 필터링합니다.
            // (최신 유니티 버전에 맞춘 무소수점 주사율 비교 방식 적용)
            if (Mathf.Approximately((float)allResolutions[i].refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
            {
                resolutions.Add(allResolutions[i]);
                string option = allResolutions[i].width + " x " + allResolutions[i].height;
                options.Add(option);

                // 현재 게임 실행 중인 해상도와 일치하는 인덱스를 찾습니다.
                if (allResolutions[i].width == Screen.currentResolution.width &&
                    allResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resolutions.Count - 1;
                }
            }
        }

        // 2. 드롭다운에 해상도 목록을 채워넣고 현재 해상도로 세팅합니다.
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // 3. 드롭다운 값이 바뀔 때 실행될 리스너 연결
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        
        // 세 번째 인자는 전체화면 여부입니다 (true: 전체화면, false: 창모드)
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
        Debug.Log($"해상도 변경 완료: {resolution.width}x{resolution.height}");
    }
}