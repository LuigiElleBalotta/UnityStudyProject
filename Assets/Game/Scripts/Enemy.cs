using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    NavMeshPath path;

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
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if( agent.pathPending ) { return; }

        if( agent.CalculatePath(targetToFollow.position, path))
        {
            agent.SetDestination(targetToFollow.position);
            return;
        }

        Debug.LogError("Il giocatore è nella safe zone.");
        //trovo un'altra destinazione temporanea

        Vector3 tmpDestination = Random.insideUnitSphere * 30;

        tmpDestination += transform.localPosition;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(tmpDestination, out hit, 1, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            Debug.Log("Entrato in collisione con player.");
        }
    }
}
