using UnityEngine;
using UnityEngine.UI;

public class FpsTracker : MonoBehaviour
{
    private Text text;
    private float time;
    private int frames;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        frames++;
        time += Time.deltaTime;

        if(frames >= 15)
        {
            text.text = (frames / time).ToString("F0");
            time = 0f;
            frames = 0;
        }
    }
}
