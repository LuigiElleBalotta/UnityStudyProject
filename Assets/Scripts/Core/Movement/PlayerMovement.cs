using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimationBase;

public class PlayerMovement : MonoBehaviour
{ 
    public Controls controls;

    Vector2 inputs;
    float rotation;
    bool run = true, jump;

    Vector3 velocity;
    float gravity = -18, velocityY, terminalVelocity = -25;

    bool jumping;
    float jumpSpeed, jumpHeight = 3;
    Vector3 jumpDirection;

    float currentSpeed;
    public float baseSpeed = 1, runSpeed = 4, rotateSpeed = 2, rotateMult = 2;

    

    UnityEngine.CharacterController controller;
    Animator anim;

    AnimationStates currentAnimation;
    bool _canMove;


    void Start()
    {
        controller = GetComponent<UnityEngine.CharacterController>();
        anim = GetComponent<Animator>();
        _canMove = true;
    }


    void Update()
    {
        GetInputs();
        Locomotion();
    }

    void Locomotion()
    {
        Vector2 inputNormalized = inputs;

        //running and walking
        currentSpeed = baseSpeed;
        if (run)
        {
            currentSpeed *= runSpeed;

            if (inputNormalized.y < 0)
            {
                currentSpeed = currentSpeed / 2;
            }
        }

        //Rotation
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        if( jump && controller.isGrounded )
        {
            Jump();
        }

        if( !controller.isGrounded && velocityY > terminalVelocity )
            velocityY += gravity * Time.deltaTime;

        //Applying inputs
        if (!jumping)
            velocity = (transform.forward * inputNormalized.y + transform.right * inputNormalized.x + Vector3.up * velocityY) * currentSpeed;
        else
            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;

        //Moving controller
        controller.Move(velocity * Time.deltaTime);

        if( controller.isGrounded )
        {
            if( jumping )
                jumping = false;

            velocityY = 0;
        }
    }

    void Jump()
    {
        if (!jumping)
            jumping = true;

        jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;

        jumpSpeed = currentSpeed;

        velocityY = Mathf.Sqrt(-gravity * jumpHeight);
    }

    void GetInputs()
    {
        //FORWARD 
        if(Input.GetKey(controls.forwards))
        {
            inputs.y = 1;
        }

        //BACKWARD
        if( Input.GetKey(controls.backwards))
        {
            if (Input.GetKey(controls.forwards))
                inputs.y = 0;
            else
                inputs.y = -1;
        }

        //LIMIT BOTH KEYS PRESSED
        if(!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
        {
            inputs.y = 0;
        }


        //STRAFE LEFT
        if (Input.GetKey(controls.strafeRight))
        {
            inputs.x = 1;
        }

        //STRAFE RIGHT
        if (Input.GetKey(controls.strafeLeft))
        {
            if (Input.GetKey(controls.strafeRight))
                inputs.x = 0;
            else
                inputs.x = -1;
        }

        //LIMIT BOTH KEYS PRESSED
        if (!Input.GetKey(controls.strafeLeft) && !Input.GetKey(controls.strafeRight))
        {
            inputs.x = 0;
        }


        //ROTATE LEFT
        if (Input.GetKey(controls.rotateRight))
        {
            rotation = 1;
        }

        //ROTATE RIGHT
        if (Input.GetKey(controls.rotateLeft))
        {
            if (Input.GetKey(controls.rotateRight))
                rotation = 0;
            else
                rotation = -1;
        }

        //LIMIT BOTH KEYS PRESSED
        if (!Input.GetKey(controls.rotateLeft) && !Input.GetKey(controls.rotateRight))
        {
            rotation = 0;
        }

        //TOGGLE RUN
        if(Input.GetKeyUp(controls.walkRun))
        {
            run = !run;
        }

        //JUMPING
        jump = Input.GetKey(controls.jump);
    }

    public bool IsRunning()
    {
        return run;
    }

    public bool CanMove()
    {
        return _canMove;
    }

    public void SetMovementEnabled(bool enabled)
    {
        _canMove = enabled;
    }
}
