using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

namespace Game.Database.Entities
{
    [SQLite4Unity3d.Table("waypoint_data")]
    public class WaypointData
    {
        [NotNull]
        public int ID { get; set; } //if negative it refers to a guid, otherwise to an id
        [NotNull]
        public int Point { get; set; } //if -1 then go back directly to the first waypoint, if -2 go reverse.
        [NotNull]
        public float PositionX { get; set; }
        [NotNull]
        public float PositionY { get; set; }
        [NotNull]
        public float PositionZ { get; set; }
        [NotNull]
        public float Orientation { get; set; }
        [NotNull]
        public int MovementType { get; set; }
        [NotNull]
        public float Delay { get; set; } = 0; //By default no delay.
    }
}

