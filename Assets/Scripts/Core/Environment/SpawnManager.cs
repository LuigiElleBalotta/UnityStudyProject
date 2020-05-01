using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Database.Entities;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Dictionary<int, CreatureTemplate> dicCreatureTemplates = new Dictionary<int, CreatureTemplate>();
    Dictionary<int, WaypointData[]> dicWaypointsPerCreature = new Dictionary<int, WaypointData[]>();

    // Start is called before the first frame update
    void Start()
    {
        dicWaypointsPerCreature = CollectionUtils.GroupBy(Global.WaypointDataList, x => x.ID);
        
        foreach(var creature in Global.CreatureList)
        {
            if( !dicCreatureTemplates.ContainsKey(creature.IDCreature))
            {
                CreatureTemplate template = Global.CreatureTemplateList.FirstOrDefault(row => row.ID == creature.IDCreature);
                dicCreatureTemplates.Add(creature.IDCreature, template);
            }
            string prefabName = dicCreatureTemplates[creature.IDCreature].PrefabName;

            GameObject go = Resources.Load($"Characters/NPCs/{prefabName}/Prefab/{prefabName}", typeof(GameObject)) as GameObject;

            GameObject spawned = Instantiate(go,
                                             new Vector3(creature.PositionX, creature.PositionY, creature.PositionZ),
                                             new Quaternion(creature.OrientationX, creature.OrientationY, creature.OrientationZ, creature.OrientationW));

            var ai = spawned.transform.GetComponent<CreatureAI>();

            if (dicWaypointsPerCreature.ContainsKey(-creature.GUID))
            {
                ai.SetWaypoints(dicWaypointsPerCreature[-creature.GUID]);
            }
                
            else if (dicWaypointsPerCreature.ContainsKey(creature.IDCreature))
            {
                ai.SetWaypoints(dicWaypointsPerCreature[creature.IDCreature]);
            }

            ai.SetCreatureDbInfo(creature, dicCreatureTemplates[creature.IDCreature]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
