using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

namespace Game.Database.Entities
{
    [SQLite4Unity3d.Table("creature_template")]
    public class CreatureTemplate
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string PrefabName { get; set; }
        public int Level { get; set; }
        public float BaseHealth { get; set; }
        public float BaseMana { get; set; }
        public string Name { get; set; }
    }
}
