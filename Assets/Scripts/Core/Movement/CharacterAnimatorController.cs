using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimationBase;

public class CharacterAnimatorController : MonoBehaviour
{
    Animator anim;
    CharacterController controller;
    PlayerMovement playerMovement;

    AnimationStates currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( controller.isGrounded )
        {

            if (Input.GetKey(KeyCode.W))
            {
                currentAnimation = playerMovement.IsRunning() ? AnimationStates.Run : AnimationStates.Walk;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentAnimation = AnimationStates.WalkBack;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //Rotate(RotationDirection.Right);
                currentAnimation = AnimationStates.RotateRight;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentAnimation = AnimationStates.RotateLeft;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                //Debug.Log("Move W+D");
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                //Debug.Log("Move W+A");
            }
            else if( Input.GetKey(KeyCode.Space))
            {
            }
            else
            {
                currentAnimation = AnimationStates.Stand;
            }
        }
        else
        {
            Debug.Log("Not grounded");
        }

        anim.SetInteger("p_currentState", (int)currentAnimation);
    }
}
