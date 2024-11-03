using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}

public class PacStudentController : MonoBehaviour
{

    float lerpDuration = 0.3f;
    float startValue = 0;
    float endValue = 1;
    float valueToLerp;
    private Direction lastInput;
    private Direction currentInput;
    private bool isLerping;
    public List<Tilemap> tilemaps;
    public Animator animatorController;
    public AudioSource audioSource;
    public AudioClip walking;
    public AudioClip pelletEaten;
    public AudioClip collision;
    public AudioClip dead;
    public ParticleSystem particle;


    // Start is called before the first frame update
    void Start()
    {
        tilemaps.Add(GameObject.Find("TopLeftQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("TopRightQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("BottomLeftQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("BottomRightQuadrant").GetComponent<Tilemap>());
        isLerping = false;
        audioSource = GetComponent<AudioSource>();
        tilemaps[0].SetTile(tilemaps[0].WorldToCell(transform.position), null);
        audioSource.clip = pelletEaten;
        audioSource.loop = false;
        audioSource.Play();
        particle.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        // Stores user input
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = Direction.UP;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = Direction.DOWN; 
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = Direction.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = Direction.RIGHT;
        }

        // Checks if player needs to move
        if (!isLerping)
        {
            if (IsWalkable(lastInput))
            {
                currentInput = lastInput;
                StartCoroutine(PlayerLerp());
            }
            else
            {
                if (IsWalkable(currentInput)) 
                {
                    StartCoroutine(PlayerLerp());
                }
                else
                {
                    particle.Pause();
                    animatorController.enabled = false;
                    audioSource.loop = false;
                }
            }
        }
    }

    // Conducts player movement
    IEnumerator PlayerLerp()
    {
        animatorController.enabled = true;
        particle.Play();
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = transform.position;
        isLerping = true;
        float timeElapsed = 0;

        // Records target position for movement
        switch (currentInput)
        {
            case Direction.UP:
                targetPosition = initialPosition + new Vector3(0, endValue, 0);
                break;
            case Direction.DOWN:
                targetPosition = initialPosition + new Vector3(0, -endValue, 0);
                break;
            case Direction.LEFT:
                targetPosition = initialPosition + new Vector3(-endValue, 0, 0);
                break;
            case Direction.RIGHT:
                targetPosition = initialPosition + new Vector3(endValue, 0, 0);
                break;
        }

        // Changes player animation
        switch (currentInput)
        {
            case Direction.LEFT:
                animatorController.SetFloat("MoveX", 1);
                animatorController.SetFloat("MoveY", 0);
                break;
            case Direction.RIGHT:
                animatorController.SetFloat("MoveX", -1);
                animatorController.SetFloat("MoveY", 0);
                break;
            case Direction.UP:
                animatorController.SetFloat("MoveX", 0);
                animatorController.SetFloat("MoveY", 1);
                break;
            case Direction.DOWN:
                animatorController.SetFloat("MoveX", 0);
                animatorController.SetFloat("MoveY", -1);
                break;
        }

        // Moves the player
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            switch (currentInput)
            {
                case Direction.UP:
                    transform.position = initialPosition + new Vector3(0, valueToLerp, 0);
                    break;
                case Direction.DOWN:
                    transform.position = initialPosition + new Vector3(0, -valueToLerp, 0);
                    break;
                case Direction.LEFT:
                    transform.position = initialPosition + new Vector3(-valueToLerp, 0, 0);
                    break;
                case Direction.RIGHT:
                    transform.position = initialPosition + new Vector3(valueToLerp, 0, 0);
                    break;
            }
           

            yield return null;
        }

        // Make sure there's no unnecessary offsets
        transform.position = targetPosition;
        isLerping = false;


        // Checks if grid contains pellet
        TileBase tile2Change = null;
        Tilemap map2Change = null;
        foreach (Tilemap map in tilemaps)
        {
            if (map.GetTile(map.WorldToCell(transform.position)) != null)
            {
                tile2Change = map.GetTile(map.WorldToCell(transform.position));
                map2Change = map;
            }
        }
        // Delete pellet consumed
        if (tile2Change != null && (tile2Change.name == "normal_pellet_0" || 
            tile2Change.name == "power_pellet_0_0"))
        {
            map2Change.SetTile(map2Change.WorldToCell(transform.position), null);
            audioSource.Stop();
            audioSource.clip = pelletEaten;
            audioSource.loop = false;
            audioSource.Play();
        } 
        else
        {
            audioSource.Stop();
            audioSource.clip = walking;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    // Checks if the desired direction is walkable
    private bool IsWalkable(Direction input)
    {
        Vector3 position = transform.position;
        TileBase tile = null;
        switch (input)
        {
            case Direction.LEFT:
                position += new Vector3(-1, 0, 0);
                break;
            case Direction.RIGHT:
                position += new Vector3(1, 0, 0);
                break;
            case Direction.UP:
                position += new Vector3(0, 1, 0);
                break;
            case Direction.DOWN:
                position += new Vector3(0, -1, 0);
                break;
        }
        foreach (Tilemap map in tilemaps)
        {
            if (map.GetTile(map.WorldToCell(position)) != null)
                tile = map.GetTile(map.WorldToCell(position));
        }
        if (tile != null)
        {
            if (tile.name == "inner_straight" ||
                tile.name == "inner_wall_corner" || 
                tile.name == "outside_wall_straight_double_0" ||
                tile.name == "outside_wall_upperleft_0" ||
                tile.name == "t_wall")
            {
                return false;
            }
        }

        return true;
    }
}
