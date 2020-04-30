using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimationBase;
using static Constants;

public class CharacterController : MonoBehaviour
{
    public float speed = 40;
    float rotSpeed = 80;
    public float gravity = -9;

    Vector3 wow_moveDir;

    Animator anim;
    UnityEngine.CharacterController controller;
    Rigidbody rb;

    bool _canMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<UnityEngine.CharacterController>();
        rb = GetComponent<Rigidbody>();

        _canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((AnimationStates)anim.GetInteger("p_currentState") + " " + DateTime.Now.ToString("DD/MM/YYYY HH:mm:ss"));
        AnimationStates animation = AnimationStates.Stand;
        if( IsGrounded() && _canMove )
        {
            if (Input.GetKey(KeyCode.W))
            {
                animation = AnimationStates.Run;

                //transform.position += transform.forward * Time.deltaTime * speed;



                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("Sta andando avanti e saltando.");

                    ForwardJump();
                }
                else if (Input.GetKey(KeyCode.A))
                    ForwardLeft();
                else if (Input.GetKey(KeyCode.D))
                    ForwardRight();
                else
                    Forward();
                    

            }
            else if (Input.GetKey(KeyCode.S))
            {
                animation = AnimationStates.WalkBack;

                transform.Translate(-transform.forward * Time.deltaTime * speed, Space.World);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Rotate(RotationDirection.Right);
                animation = AnimationStates.RotateRight;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Rotate(RotationDirection.Left);

                animation = AnimationStates.RotateLeft;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                //anim.SetInteger("p_currentState", (int)AnimationStates.Run);
                //transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
                //anim.SetInteger("p_currentState", (int)AnimationStates.RotateRight);
                Debug.Log("Move W+D");
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                //anim.SetInteger("p_currentState", (int)AnimationStates.Run);
                //transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
                //transform.Rotate(0, -1, 0, Space.Self);
                Debug.Log("Move W+A");
            }
            else if( Input.GetKey(KeyCode.Space))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
                //animation = AnimationStates.Jump;
            }
            else
            {
                animation = AnimationStates.Stand;
            }
        }
        else
        {
            //anim.SetInteger("p_currentState", (int)AnimationStates.FallDown);
            Debug.Log("Not grounded");
        }

        //if(!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).loop))
        anim.SetInteger("p_currentState", (int)animation);
    }

    private void Rotate(RotationDirection direction)
    {
        if( direction == RotationDirection.Left )
        {
            transform.Rotate(0, -1, 0, Space.Self);
        }
        else
        {
            transform.Rotate(0, 1, 0, Space.Self);
        }
    }

    private void ForwardLeft()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        Rotate(RotationDirection.Left);
    }

    private void ForwardRight()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        Rotate(RotationDirection.Right);
    }

    private void ForwardJump()
    {
        rb.AddForce(0, 0, 1.0f, ForceMode.Impulse);
    }

    private void Forward()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.0f + 0.1f);
    }

    public void SetMovementEnabled( bool enabled )
    {
        _canMove = enabled;
    }
}
