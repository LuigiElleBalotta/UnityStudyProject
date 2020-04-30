using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    Transform targetToFollow;

    void OnValidate()
    {
        if(!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetToFollow.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthManager>().Damage(999);
        }
    }
}
