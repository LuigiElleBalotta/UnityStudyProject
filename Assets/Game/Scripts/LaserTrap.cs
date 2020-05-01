using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRederer;
    [SerializeField]
    int damage = 10;

    void OnValidate()
    {
        if(lineRederer) { return; }
        lineRederer = GetComponentInChildren<LineRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Trigger(other);
    }

    void Trigger(Collider other)
    {
        PlayerStats hm = other.GetComponent<PlayerStats>();

        if (!hm) { return; }

        hm.ReceiveDamage(damage);
    }
}
