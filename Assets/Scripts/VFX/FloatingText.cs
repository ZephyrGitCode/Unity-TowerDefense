using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float speed = 10f;
    public float fadeDuration = 5f;
    public float age = 0f;
    private TMP_Text text;
    private Color initialColor;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        initialColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        // fade over time
        age += Time.deltaTime;
        float halfway = Mathf.Lerp(10f, 20f, 0.5f);
        text.color = Color.Lerp(initialColor, Color.clear, age / fadeDuration);

        if (age > fadeDuration)
        {
            Destroy(this.gameObject);
        }
    }
}
