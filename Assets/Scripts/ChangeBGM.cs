using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public AudioSource BGM;

    public AudioClip normal_BGM;

    const float changeBGM = 3.0f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > changeBGM)
        {
            BGM.Stop();
            BGM.clip = normal_BGM;
            BGM.Play();
        }
    }
}
