using System;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    public RectTransform identityMask;
    private float maskFullHeight;
    public GameObject identityBackGround;

    private const float MAX_IDENTITY = 200f;

    public float identity;

    void Start()
    {
        maskFullHeight = identityMask.sizeDelta.y;
        UpdateIdentityBar();
    }

    public void AddIdentity(float value)
    {
        identity += value;
        identity = Math.Min(identity, MAX_IDENTITY);
    }

    public void SetIdentity(float value)
    {
        identity = value;
    }

    public void UpdateIdentityBar()
    {
        float identityRatio = Mathf.Clamp01(identity / MAX_IDENTITY);
        identityMask.sizeDelta = new Vector2(identityMask.sizeDelta.x, maskFullHeight * identityRatio);
    }

    public void SetIdentityReady(bool show)
    {
        identityBackGround.SetActive(show);
    }

}
