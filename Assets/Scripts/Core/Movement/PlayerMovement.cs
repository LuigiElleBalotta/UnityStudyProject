using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimationBase;
using static Constants;

public class PlayerMovement : MonoBehaviour
{
    //inputs
    public Controls controls;
    Vector2 inputs;
    [HideInInspector]
    public Vector2 inputNormalized;
    [HideInInspector]
    public float rotation;
    bool run = true, jump;
    [HideInInspector]
    public bool steer, autoRun;
    public LayerMask waterMask;
    public MoveState moveState = MoveState.locomotion;

    //velocity
    Vector3 velocity;
    float gravity = -18, velocityY, terminalVelocity = -25;
    float fallMult;

    //Running
    float currentSpeed;
    [HideInInspector]
    public float baseSpeed = 1, runSpeed = 4, rotateSpeed = 2;

    //ground
    Vector3 forwardDirection, collisionPoint;
    float slopeAngle, directionAngle, forwardAngle, strafeAngle;
    float forwardMult, strafeMult;
    Ray groundRay;
    RaycastHit groundHit;

    //mounted
    public bool mount;
    [HideInInspector]
    public MountType mountType;
    [HideInInspector]
    public MountedState mountedState = MountedState.unmounted;
    [HideInInspector]
    public float mountedSpeed = 1.6f, flyingSpeed = 2.5f;

    //Jumping
    bool jumping, canJump = true;
    float jumpStartPosY;
    float jumpSpeed, jumpHeight = 3;
    Vector3 jumpDirection;

    //swimming
    float swimSpeed = 2, swimLevel = 25;
    [HideInInspector]
    public float waterSurface, d_fromWaterSurface;
    public bool inWater;

    //references
    CharacterController controller;
    public Transform groundDirection, moveDirection, fallDirection, swimDirection;


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
        GetSwimDirection();

        if (inWater)
            GetWaterlevel();

        if( mount)
        {
            if( mountType == MountType.Ground)
            {
                if( mountedState != MountedState.mounted )
                    mountedState = MountedState.mounted;
            }
            else if( mountType == MountType.Flying)
            {
                if(mountedState != MountedState.mountedFlying)
                    mountedState = MountedState.mountedFlying;
            }
        }
        else
        {
            if( mountedState != MountedState.unmounted )
                mountedState = MountedState.unmounted;
        }

        switch (moveState)
        {
            case MoveState.locomotion:
                Locomotion();
                break;

            case MoveState.swimming:
                Swimming();
                break;

            case MoveState.flying:
                Flying();
                break;
        }
    }

    void Locomotion()
    {
        GroundDirection();

        //running and walking
        if (controller.isGrounded && slopeAngle <= controller.slopeLimit)
        {
            currentSpeed = baseSpeed;

            if (run)
            {
                currentSpeed *= runSpeed;

                if( mountedState != MountedState.unmounted )
                {
                    currentSpeed *= mountedSpeed;
                }

                if (inputNormalized.y < 0)
                    currentSpeed = currentSpeed / 2;
            }
        }
        else if (!controller.isGrounded || slopeAngle > controller.slopeLimit)
        {
            inputNormalized = Vector2.Lerp(inputNormalized, Vector2.zero, 0.025f);
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.025f);
        }

        //Rotating
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        //Press space to Jump
        if ( jump && controller.isGrounded && slopeAngle <= controller.slopeLimit && canJump )
            Jump();

        //apply gravity if not grounded
        if (!controller.isGrounded && velocityY > terminalVelocity)
        {
            

            switch(mountedState)
            {
                case MountedState.unmounted:
                case MountedState.mounted:
                    if( velocityY > terminalVelocity )
                        velocityY += gravity * Time.deltaTime;
                    break;
                case MountedState.mountedFlying:
                    if (Physics.Raycast(groundRay, out groundHit, 0.15f, waterMask))
                    {
                        if (velocityY > terminalVelocity)
                            velocityY += gravity * Time.deltaTime;
                    }
                    else
                    {
                        moveState = MoveState.flying;
                    }  
                    break;
            }
        }
        else if (controller.isGrounded && slopeAngle > controller.slopeLimit)
            velocityY = Mathf.Lerp(velocityY, terminalVelocity, 0.25f);

        //checking WaterLevel
        if (inWater)
        {
            //setting ground ray
            groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
            groundRay.direction = Vector3.down;

            //if (Physics.Raycast(groundRay, out groundHit, 0.15f))
            //    currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, d_fromWaterSurface / swimLevel);

            if (d_fromWaterSurface >= swimLevel)
            {
                if (jumping)
                    jumping = false;

                moveState = MoveState.swimming;
            }
                
        }

        //Applying inputs
        if (!jumping)
        {
            velocity = groundDirection.forward * inputNormalized.y * forwardMult + groundDirection.right * inputNormalized.x * strafeMult; //Applying movement direction inputs
            velocity *= currentSpeed; //Applying current move speed
            velocity += fallDirection.up * (velocityY * fallMult); //Gravity
        }
        else
            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;

        //moving controller
        controller.Move(velocity * Time.deltaTime);

        if(mountedState == MountedState.mountedFlying)
        {
            float currentJumpHeight = transform.position.y - jumpStartPosY;

            if (currentJumpHeight > 0.5f)
                moveState = MoveState.flying;
        }

        if (controller.isGrounded)
        {
            //stop jumping if grounded
            if (jumping)
                jumping = false;

            if (!jump && !canJump)
                canJump = true;

            // stop gravity if grounded
            velocityY = 0;
        }
    }

    void GroundDirection()
    {
        //SETTING FORWARDDIRECTION
        //Setting forwardDirection to controller position
        forwardDirection = transform.position;

        //Setting forwardDirection based on control input.
        if (inputNormalized.magnitude > 0)
            forwardDirection += transform.forward * inputNormalized.y + transform.right * inputNormalized.x;
        else
            forwardDirection += transform.forward;

        //Setting groundDirection to look in the forwardDirection normal
        moveDirection.LookAt(forwardDirection);
        fallDirection.rotation = transform.rotation;
        groundDirection.rotation = transform.rotation;

        //setting ground ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;


        forwardMult = 1;
        fallMult = 1;
        strafeMult = 1;

        if (Physics.Raycast(groundRay, out groundHit, 0.3f, waterMask))
        {
            //Getting angles
            slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
            directionAngle = Vector3.Angle(moveDirection.forward, groundHit.normal) - 90;

            if (directionAngle < 0 && slopeAngle <= controller.slopeLimit)
            {
                forwardAngle = Vector3.Angle(transform.forward, groundHit.normal) - 90; //Chekcing the forwardAngle against the slope
                forwardMult = 1 / Mathf.Cos(forwardAngle * Mathf.Deg2Rad); //Applying the forward movement multiplier based on the forwardAngle
                groundDirection.eulerAngles += new Vector3(-forwardAngle, 0, 0); //Rotating groundDirection X

                strafeAngle = Vector3.Angle(groundDirection.right, groundHit.normal) - 90; //Checking the strafeAngle against the slope
                strafeMult = 1 / Mathf.Cos(strafeAngle * Mathf.Deg2Rad); //Applying the strafe movement multiplier based on the strafeAngle
                groundDirection.eulerAngles += new Vector3(0, 0, strafeAngle);
            }
            else if (slopeAngle > controller.slopeLimit)
            {
                float groundDistance = Vector3.Distance(groundRay.origin, groundHit.point);

                if (groundDistance <= 0.1f)
                {
                    fallMult = 1 / Mathf.Cos((90 - slopeAngle) * Mathf.Deg2Rad);

                    Vector3 groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
                    fallDirection.rotation = Quaternion.FromToRotation(transform.up, Vector3.Cross(groundCross, groundHit.normal));
                }
            }
        }
    }

    void Jump()
    {
        //set Jumping to true
        if (!jumping)
        {
            jumpStartPosY = transform.position.y;
            jumping = true;
            canJump = false;
        }

        switch(moveState)
        {
            case MoveState.locomotion:
                //Set jump direction and speed
                jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;
                jumpSpeed = currentSpeed;

                //set velocity Y
                velocityY = Mathf.Sqrt(-gravity * jumpHeight);
                break;

            case MoveState.swimming:
                //Set jump direction and speed
                jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;
                jumpSpeed = swimSpeed;

                //set velocity Y
                velocityY = Mathf.Sqrt(-gravity * jumpHeight * 1.25f);
                break;
        }
    }

    void GetInputs()
    {
        //FORWARDS BACKWARDS CONTROLS  
        inputs.y = Axis(controls.forwards.GetControlBinding(), controls.backwards.GetControlBinding());


        if (autoRun)
        {
            inputs.y += Axis(true, false);

            inputs.y = Mathf.Clamp(inputs.y, -1, 1);
        }

        //STRAFE LEFT RIGHT
        inputs.x = Axis(controls.strafeRight.GetControlBinding(), controls.strafeLeft.GetControlBinding());

        if (steer)
        {
            inputs.x += Axis(controls.rotateRight.GetControlBinding(), controls.rotateLeft.GetControlBinding());

            inputs.x = Mathf.Clamp(inputs.x, -1, 1);
        }

        //ROTATE LEFT RIGHT
        /*if (steer)
            rotation = Input.GetAxis("Mouse X") * mainCam.CameraSpeed;
        else
            rotation = Axis(controls.rotateRight.GetControlBinding(), controls.rotateLeft.GetControlBinding());
            */

        rotation = Axis(controls.rotateRight.GetControlBinding(), controls.rotateLeft.GetControlBinding());

        //ToggleRun
        if (controls.walkRun.GetControlBindingDown())
            run = !run;

        //Jumping
        jump = controls.jump.GetControlBinding();

        inputNormalized = inputs.normalized;
    }

    void GetSwimDirection()
    {
        /*
        if (steer)
            swimDirection.eulerAngles = transform.eulerAngles + new Vector3(mainCam.tilt.eulerAngles.x, 0, 0);
            */
    }

    void Swimming()
    {
        if(!inWater && !jumping)
        {
            velocityY = 0;
            velocity = new Vector3(velocity.x, velocityY, velocity.z);
            jumpDirection = velocity;
            jumpSpeed = swimSpeed / 2;
            jumping = true;
            moveState = MoveState.locomotion;
        }

        //Rotating
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        //setting ground ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        if (!jumping && jump && d_fromWaterSurface <= swimLevel)
            Jump();

        if( !jumping )
        {
            velocity = swimDirection.forward * inputNormalized.y + swimDirection.right * inputNormalized.x;

            velocity.y += Axis(jump, controls.sit.GetControlBinding());

            velocity = velocity.normalized;

            velocity *= swimSpeed;

            controller.Move(velocity * Time.deltaTime);

            if (Physics.Raycast(groundRay, out groundHit, 0.15f))
            {
                Debug.Log($"[FROM if !jumping] d_fromWaterSurface: {d_fromWaterSurface}, swimLevel: {swimLevel}");
                if (d_fromWaterSurface < swimLevel)
                    moveState = MoveState.locomotion;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, float.MinValue, waterSurface - swimLevel), transform.position.z);
            }
        }
        else
        {
            //jump
            if( velocityY > terminalVelocity )
            {
                velocityY += gravity * Time.deltaTime;
            }

            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            if (mountedState == MountedState.mountedFlying)
            {
                float currentJumpHeight = transform.position.y - jumpStartPosY;

                if (currentJumpHeight > 0.5f)
                    moveState = MoveState.flying;
            }

            if (Physics.Raycast(groundRay, out groundHit, 0.15f))
            {
                Debug.Log($"[FROM //JUMP ] d_fromWaterSurface: {d_fromWaterSurface}, swimLevel: {swimLevel}");
                if (d_fromWaterSurface < swimLevel)
                    moveState = MoveState.locomotion;
            }

            if (d_fromWaterSurface > swimLevel)
                jumping = false;
        }
    }

    void Flying()
    {
        if (mountedState == MountedState.unmounted)
            moveState = MoveState.locomotion;

        if( jumping )
        {
            velocityY = 0;
            jumping = false;
        }

        //Rotating
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        //setting ground ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        velocity = swimDirection.forward * inputNormalized.y + swimDirection.right * inputNormalized.x;

        velocity.y += Axis(jump, controls.sit.GetControlBinding());

        velocity = velocity.normalized;

        velocity *= runSpeed * flyingSpeed;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            moveState = MoveState.locomotion;
        }

        if (inWater && d_fromWaterSurface >= swimLevel)
            moveState = MoveState.swimming;

    }

    void GetWaterlevel()
    {
        d_fromWaterSurface = waterSurface - transform.position.y;
        //d_fromWaterSurface = Mathf.Clamp(d_fromWaterSurface, 0, float.MaxValue);
    }

    public float Axis(bool pos, bool neg)
    {
        float axis = 0;

        if (pos)
            axis += 1;

        if (neg)
            axis -= 1;

        return axis;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y <= transform.position.y + 0.25f)
        {
            collisionPoint = hit.point;
            collisionPoint = collisionPoint - transform.position;
        }
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

