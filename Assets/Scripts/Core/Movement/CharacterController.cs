using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimationBase;

public class CharacterController : MonoBehaviour
{
    public float speed = 40;
    float rotSpeed = 80;
    public float gravity = 8;

    Vector3 moveDir = Vector3.zero;

    //UnityEngine.CharacterController controller;
    //Transform playerContainer;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponentInChildren<UnityEngine.CharacterController>();
        anim = GetComponentInChildren<Animator>();
        //playerContainer = GameObject.FindGameObjectWithTag("PlayerContainer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            
            moveDir = Vector3.forward;
            moveDir *= speed;
            anim.SetInteger("p_currentState", (int)AnimationStates.Run);

            transform.Translate(moveDir, Space.World);
            
        }
        else if( Input.GetKey(KeyCode.S))
        {
            moveDir = Vector3.back;
            moveDir *= speed;
            anim.SetInteger("p_currentState", (int)AnimationStates.WalkBack);

            transform.Translate(moveDir, Space.World);
        }
        else if( Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0, Space.Self);
            anim.SetInteger("p_currentState", (int)AnimationStates.RotateRight);
        }
        else if( Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0, Space.Self);
            anim.SetInteger("p_currentState", (int)AnimationStates.RotateLeft);
        }
        else
        {
            anim.SetInteger("p_currentState", (int)AnimationStates.Stand);
            moveDir = Vector3.zero;
        }

        moveDir.y -= gravity * Time.deltaTime;

        //controller.Move(moveDir);

        //playerContainer.transform.position = transform.position;



    }
}
