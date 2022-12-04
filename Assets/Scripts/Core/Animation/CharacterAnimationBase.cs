using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationBase : MonoBehaviour
{
    public enum AnimationStates
    {
        Stand = 0,
        Walk = 1,
        Run = 2,
        WalkBack = 3,
        RotateLeft = 4,
        RotateRight = 5,
        Jump = 6,
        FallDown = 7,
        SwimIdle = 8,
        SwimBack = 9,
        SwimLeft = 10,
        SwimRight = 11,
        SwimForward = 12
    }

    public AnimationStates currentState;

    [SerializeField] private Animator p_Animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterAnimation()
    {
        Animator.SetInteger("p_currentState", (int)AnimationStates.Stand);
    }

    public Animator Animator
    {
        get
        {
            if (p_Animator == null)
                p_Animator = GetComponentInChildren<Animator>();

            return p_Animator;
        }
    }
}
