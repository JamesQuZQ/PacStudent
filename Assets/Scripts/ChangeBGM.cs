using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BGM_Type{
    INTRO,
    NORMAL,
    SCARED,
    GHOST_EATEN,
    STOP
}


public class ChangeBGM : MonoBehaviour
{
    public AudioSource BGM;

    public AudioClip NormalBGM;

    private BGM_Type bgmType;

    const float changeBGM = 6.0f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        bgmType = BGM_Type.INTRO;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (bgmType == BGM_Type.INTRO && timer > changeBGM)
        {
            bgmType = BGM_Type.NORMAL;
            BGM.Stop();
            BGM.clip = NormalBGM;
            BGM.Play();
        }
    }
}
