using System.Collections;
using System.Collections.Generic;
using Game.Database.Entities;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : CreatureStats
{
    bool _initialized;
    public Creature creatureDbInfo;
    public CreatureTemplate creatureTemplate;

    WaypointData[] _waypoints;
    bool isDoingWaypoints;
    int currentWaypointIdx;
    [SerializeField]
    NavMeshAgent agent;

    NavMeshPath path;

    Vector3 currentEndpointPosition;

    void OnValidate()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        currentWaypointIdx = 0;
    }

    void Start()
    {
        path = new NavMeshPath();
        currentEndpointPosition = Vector3.zero;

        _initialized = false;
        
        
    }

    void InitCreatureStats()
    {
        maxHp = creatureTemplate.BaseHealth;
        maxMana = creatureTemplate.BaseMana;
        level = creatureTemplate.Level;
        name = creatureTemplate.Name;
    }

    void Update()
    {
        if(!_initialized)
        {
            if( creatureDbInfo != null && creatureTemplate != null)
            {
                _initialized = true;
                InitCreatureStats();
            }
        }

        if(!isDead)
        {
            if(!_isInCombat)
            {
                if(!isDoingWaypoints)
                {
                    if (_waypoints != null && _waypoints.Length > 0)
                    {
                        isDoingWaypoints = true;
                    }
                }
                else
                {
                    float dist = agent.remainingDistance;
                    if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
                    {
                        //arrived
                        currentEndpointPosition = new Vector3(_waypoints[currentWaypointIdx].PositionX,
                                                              _waypoints[currentWaypointIdx].PositionY,
                                                              _waypoints[currentWaypointIdx].PositionZ);

                        if (agent.CalculatePath(currentEndpointPosition, path))
                        {
                            agent.SetDestination(currentEndpointPosition);
                            currentWaypointIdx++;
                            if (currentWaypointIdx == _waypoints.Length)
                                currentWaypointIdx = 0;
                            return;
                        }
                    } 
                }
            }
        }
    }

    public void SetWaypoints(WaypointData[] waypoints)
    {
        _waypoints = waypoints;
        Debug.Log($"Set new waypoints for this creature: \"{gameObject.name}\"");
    }

    public void SetCreatureDbInfo( Creature c, CreatureTemplate ct )
    {
        creatureDbInfo = c;
        creatureTemplate = ct;
    }
}
