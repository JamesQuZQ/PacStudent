using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class CherryGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;
    bool timerUpdate;
    int spawnDuration = 10;
    GameObject cherry;
    int lerpDuration = 20;
    float valueToLerpX;
    float valueToLerpY;
    float startValueX;
    float endValueX;
    float startValueY;
    float endValueY;

    void Start()
    {
        timerUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerUpdate) 
        {
            time += Time.deltaTime;
        }
        if (time > spawnDuration)
        {
            SpawnCherry();
            time = 0;
            timerUpdate = false;
            StartCoroutine(CherryLerp());
        }
    }

    private void SpawnCherry()
    {
        //Create a new gameObject
        cherry = new GameObject("cherry");
        //Add an SpriteRenderer component
        SpriteRenderer char_sprite = cherry.AddComponent<SpriteRenderer>();
        //Load the sprite and assign it
        char_sprite.sprite = Resources.Load<Sprite>("Sprites/Pellets/bonus_score_cherry");
        if (Random.Range(0,2) == 0)
        {
            startValueX = Random.Range(-0.1f,1.1f);
            startValueY = Random.Range(0, 2) == 0 ? Random.Range(1f, 1.1f) : Random.Range(-0.1f, 0);
        } 
        else
        {
            startValueY = Random.Range(-0.1f, 1.1f);
            startValueX = Random.Range(0, 2) == 0 ? Random.Range(1f, 1.1f) : Random.Range(-0.1f, 0);
        }
        cherry.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(startValueX, startValueY, 9.0f));
        cherry.transform.localScale = new Vector3(3, 3, 3);
        //cherry.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 20.0f));

    }

    IEnumerator CherryLerp()
    {
        float timeElapsed = 0;
        Vector3 initialPosition = cherry.transform.position;
        float endValueX = 1 - startValueX;
        float endValueY = 1 - startValueY;


        while (timeElapsed < lerpDuration)
        {
            valueToLerpX = Mathf.Lerp(startValueX, endValueX, timeElapsed / lerpDuration);
            valueToLerpY = Mathf.Lerp(startValueY, endValueY, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            cherry.transform.position = Camera.main.ViewportToWorldPoint(
                new Vector3(valueToLerpX, valueToLerpY, 9.0f));

            yield return null;
        }

        // Make sure there's no unnecessary offsets
        cherry.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(endValueX, endValueY, 0));

        // Destroy cherry
        Destroy(cherry);
        timerUpdate = true;
    }
}
