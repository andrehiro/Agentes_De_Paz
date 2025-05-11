using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float fadeDuration = 1f;

    public TextMeshProUGUI text;
    private Color originalColor;
    private float timer = 0f;
    private Vector3 startLocalPosition;

    void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        originalColor = text.color;
        startLocalPosition = transform.localPosition;
    }

    void OnEnable()
    {
        transform.localPosition = startLocalPosition;
        text.color = originalColor;
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        timer += Time.deltaTime;

        Color c = text.color;
        c.a = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
        text.color = c;

        if (timer >= fadeDuration)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}