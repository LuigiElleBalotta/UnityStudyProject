using System.Collections.Generic;
using Game.Database.Entities;
using UnityEngine;

public class BeforeSceneStartScript
{
    static SQLite4Unity3d.SQLiteConnection database = null;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log(System.IO.Directory.GetCurrentDirectory());
        Constants.db = new SQLite4Unity3d.SQLiteConnection("./Assets/StreamingAssets/game.db");

        database = Constants.db;

        database.CreateTable<CreatureTemplate>();
        database.CreateTable<Creature>();
        database.CreateTable<WaypointData>();

        var rows = database.Table<CreatureTemplate>().Where(row => row.PrefabName == "WarlockUndead");
        if( rows.Count() == 0 )
        {
            database.Insert(new CreatureTemplate { Level = 1, PrefabName = "WarlockUndead", BaseHealth = 200, BaseMana = 100, Name = "Warlock Citizien" });
        }

        var creature_templates = database.Table<CreatureTemplate>();
        Global.CreatureTemplateList.AddRange(creature_templates);

        Debug.Log($"Loaded {Global.CreatureTemplateList.Count} creature templates.");

        var creatures = database.Table<Creature>();
        Global.CreatureList.AddRange(creatures);

        Debug.Log($"Loaded {Global.CreatureList.Count} creatures.");

        var waypointsdata = database.Table<WaypointData>().OrderBy(x => x.ID).ThenBy(x => x.Point);
        Global.WaypointDataList.AddRange(waypointsdata);
    }
}
