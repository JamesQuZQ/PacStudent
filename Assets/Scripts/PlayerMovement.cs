using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum Direction {
    UP,
    DOWN, 
    LEFT,
    RIGHT,
    NONE
}

public class PlayerMovement : MonoBehaviour
{
    Direction direction = Direction.NONE;

    public Animator animatorController;

    private float timer;

    const float changeDirection = 3.0f;

    private float lastTime;

    void Start()
    {
        timer = 0;
        lastTime = -0.1f;
        direction = Direction.LEFT;
    }

    void Update()
    {
        if (timer > lastTime + 0.1f)
        {
            lastTime += 0.1f;
            switch (direction)
            {
                case Direction.UP:
                    break;
                case Direction.DOWN:
                    break;
                case Direction.LEFT:
                    transform.position = transform.position + new Vector3(-0.2f, 0);
                    break;
                case Direction.RIGHT:
                    transform.position = transform.position + new Vector3(0.2f, 0);
                    break;
            }
        }


        timer += Time.deltaTime;
        if (timer >= changeDirection)
        {
            if (direction == Direction.LEFT)
            {
                direction = Direction.RIGHT;
                animatorController.SetTrigger("RotateRightTrigger");
            }
            else if (direction == Direction.RIGHT)
            {
                direction = Direction.LEFT;
                animatorController.SetTrigger("RotateLeftTrigger");
            }
            timer = 0;
            lastTime = -0.1f;
        }


            //if (Input.GetKeyDown(KeyCode.UpArrow)) 
            //{ 
            //    direction = Direction.UP;
            //} else if (Input.GetKeyDown(KeyCode.DownArrow)) 
            //{
            //    animatorController.SetTrigger("RotateDownParam");
            //    direction = Direction.DOWN; 
            //}
    }
}
