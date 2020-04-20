using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Vector3 WowForward = new Vector3(1, 0, 0);
    public static Vector3 WowBack = new Vector3(-1, 0, 0);

    public enum RotationDirection
    {
        Left = 1,
        Right = 2
    }
}
