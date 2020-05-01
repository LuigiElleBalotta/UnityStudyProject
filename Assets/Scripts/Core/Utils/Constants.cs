using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Vector3 WowForward = new Vector3(1, 0, 0);
    public static Vector3 WowBack = new Vector3(-1, 0, 0);

    public static SQLite4Unity3d.SQLiteConnection db;

    public enum RotationDirection
    {
        Left = 1,
        Right = 2
    }

    public enum TargetSelectionType
    {
        SelectOnly = 0,
        SelectAndAttack = 1
    }

    public enum MovementType
    {
        Walk = 1,
        Run = 2
    }

    public enum GameobjectType
    {
        None = 0,
        Player = 1,
        Creature = 2,
        Gameobject = 3
    }


}
