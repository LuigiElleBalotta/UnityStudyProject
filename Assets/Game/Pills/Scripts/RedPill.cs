using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPill : Pill
{
    public int damage = 10;
    protected override void OnPicked(Collider other)
    {
        base.OnPicked(other);

        PlayerStats hm = other.GetComponent<PlayerStats>();

        if(!hm) { return; }

        hm.ReceiveDamage(damage);

        Destroy(gameObject, 2);

    }
}
