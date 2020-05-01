using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollisions : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if( collision.gameObject.tag == "Platform")
        {
            transform.parent = GameObject.Find("OrgrimmarElevatorPlatform").transform;
        }
    }

    void OnCollisionExit()
    {
        transform.parent = null;
    }
}
