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

    const float changeToHorizontalDirection = 2.0f;

    const float changeToVerticalDirection = 2.5f;

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
                    transform.position = transform.position + new Vector3(0, 0.2f);
                    break;
                case Direction.DOWN:
                    transform.position = transform.position + new Vector3(0, -0.2f);
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
        if (direction == Direction.LEFT || direction == Direction.RIGHT)
        {
            if (timer >= changeToVerticalDirection)
            {
                if (direction == Direction.LEFT)
                {
                    direction = Direction.UP;
                    animatorController.SetTrigger("MoveUpTrigger");
                }
                else
                {
                    direction = Direction.DOWN;
                    animatorController.SetTrigger("MoveDownTrigger");
                }
                timer = 0;
                lastTime = -0.1f;
            }
        }
        else if (direction == Direction.UP || direction == Direction.DOWN)
        {
            if (timer >= changeToHorizontalDirection)
            {
                if (direction == Direction.UP)
                {
                    direction = Direction.RIGHT;
                    animatorController.SetTrigger("MoveRightTrigger");
                }

                else
                {
                    direction = Direction.LEFT;
                    animatorController.SetTrigger("MoveLeftTrigger");
                }
                timer = 0;
                lastTime = -0.1f;
            }
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
