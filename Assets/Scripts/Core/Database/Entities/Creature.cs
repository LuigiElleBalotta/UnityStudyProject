using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Game.Database.Entities
{
    [SQLite4Unity3d.Table("creatures")]
    public class Creature
    {
        [PrimaryKey, AutoIncrement]
        public int GUID { get; set; }

        public int IDCreature { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float OrientationX { get; set; }
        public float OrientationY { get; set; }
        public float OrientationZ { get; set; }
        public float OrientationW { get; set; }

    }
}

