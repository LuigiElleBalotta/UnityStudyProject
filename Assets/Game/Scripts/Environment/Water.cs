using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Transform floorLevel;
    BoxCollider col;

    void Start()
    {
        floorLevel = transform.GetChild(0);
        col = GetComponent<BoxCollider>();

        AdjustCollider();
    }

    public void AdjustCollider()
    {
        float waterSize = Vector3.Distance(transform.position, floorLevel.position);
        float waterCenter = waterSize / 2;

        col.size = new Vector3(col.size.x, waterSize + 0.25f, col.size.z);
        col.center = new Vector3(0, -waterCenter, 0);
    }
}
