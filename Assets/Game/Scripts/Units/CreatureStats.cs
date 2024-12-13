using System.Collections;
using System.Collections.Generic;
using Game.Database.Entities;
using UnityEngine;

public class CreatureStats : UnitStats
{
    void Start()
    {
        Type = Constants.GameobjectType.Creature;
    }

    void Update()
    {
        base.ManagePowers();
        base.ManageSelectedUnit();
    }
}
