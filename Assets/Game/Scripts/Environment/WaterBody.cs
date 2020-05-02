using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerStay(Collider other)
    {
        if( other.GetComponent<PlayerMovement>() == player )
        {
            if (!player.inWater)
                player.inWater = true;

            if (player.waterSurface != transform.position.y)
                player.waterSurface = transform.position.y;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == player)
        {
            if (player.inWater)
                player.inWater = false;
        }
    }
}
