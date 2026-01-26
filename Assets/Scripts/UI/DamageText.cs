using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 40f;
    public float lifeTime = 1f;

    private TextMeshProUGUI text;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Init(float value)
    {
        text.text = Mathf.RoundToInt(value).ToString();
        timer = lifeTime;
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if (timer <= 0f)
            Destroy(gameObject);
    }
}
