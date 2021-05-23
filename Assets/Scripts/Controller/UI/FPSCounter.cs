using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))] //per non avere una null reference nello start() dove prendo il text component
public class FPSCounter : MonoBehaviour
{
    private Text textComponent;
    private int frameCount = 0;
    private float fps = 0;
    private float timeLeft;
    private float accum = 0f;
    private float updateInterval = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        timeLeft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount += 1; //Counter of the number of frames rendered
        timeLeft -= Time.deltaTime; // Left time for the current interval
        //the number of FPS accumulated over the interval
        accum += Time.timeScale / Time.deltaTime;
        if (timeLeft <= 0f)
        {
            fps = accum / frameCount;
            timeLeft = updateInterval;
            accum = 0f;
            frameCount = 0;
        }
        if (fps < 30) { textComponent.color = Color.red; }
        else if (fps < 60) { textComponent.color = Color.yellow; }
        else { textComponent.color = Color.green; }
        textComponent.text = fps.ToString("F2"); // F2 to rapresent two digit after comma
    }
}

