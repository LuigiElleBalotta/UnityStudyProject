using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePill : Pill
{
    public int healAmount = 10;
    protected override void OnPicked(Collider other)
    {
        base.OnPicked(other);

        PlayerStats hm = other.GetComponent<PlayerStats>();

        if (!hm) { return; }

        hm.ReceiveDamage(-healAmount);

        Destroy(gameObject, 2);

    }
}
