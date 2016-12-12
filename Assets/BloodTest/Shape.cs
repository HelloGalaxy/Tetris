using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public Text text;
    public Slider slider;

    public float value = 100;

    public float deltaTime = 0f;
    public float threhold = 1f;

    private void Awake()
    {
        threhold = 3;// Random.Range(3f, 5f);
    }

    private void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > threhold)
        {
            text.gameObject.SetActive(true);
            value -= Random.Range(1f, 10f);
            text.text = value.ToString();
            slider.value = value / 100f;
            deltaTime = 0;
            threhold = 3; Random.Range(3f, 5f);
        }
        if (deltaTime >= 2)
        {
            text.gameObject.SetActive(false);
        }
    }
}
