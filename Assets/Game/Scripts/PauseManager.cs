using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Database.Entities;
using UnityEngine;
using static Constants;

public class PauseManager : MonoBehaviour
{
    bool isPaused;
    public GameObject pnlPause;

    int lastPoint = 0;

    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyUp(KeyCode.Escape))
        {
            PlayerStats hm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            if (hm.HasTarget())
            {
                hm.SetTarget(null);
            }
            else
            {
                //attivare o disattivare la pausa
                ChangePauseStatus();
            }
        }
    }

    void UpdateGamePause()
    {
        if( isPaused )
        {
            //ferma il tempo
            Time.timeScale = 0;
        }
        else
        {
            //riavvia il tempo
            Time.timeScale = 1;
        }

        pnlPause.SetActive(isPaused);
    }

    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }

    public void SpawnWarlock()
    {
        Transform currentPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        Creature creature = new Creature
        {
            IDCreature = Global.CreatureTemplateList.FirstOrDefault(row => row.PrefabName == "WarlockUndead").ID,
            PositionX = currentPlayer.position.x,
            PositionY = currentPlayer.position.y,
            PositionZ = currentPlayer.position.z,
            OrientationX = currentPlayer.rotation.x,
            OrientationY = currentPlayer.rotation.y,
            OrientationZ = currentPlayer.rotation.z,
            OrientationW = currentPlayer.rotation.w
        };

        creature.GUID = Constants.db.Insert(creature);

        Global.CreatureList.Add(creature);

        ChangePauseStatus();
    }

    public void AddWaypoint()
    {
        Transform currentPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        var selectedUnit = currentPlayer.GetComponent<PlayerStats>().selectedUnit;
        if (!selectedUnit)
        {
            Debug.LogError("No target");
        }
        else
        {
            var ai = selectedUnit.GetComponent<CreatureAI>();
            WaypointData wd = new WaypointData
            {
                ID = -(ai?.creatureDbInfo.GUID) ?? 0,
                Delay = 0,
                MovementType = (int)MovementType.Walk,
                Point = lastPoint,
                PositionX = currentPlayer.position.x,
                PositionY = currentPlayer.position.y,
                PositionZ = currentPlayer.position.z,
                Orientation = currentPlayer.rotation.y
            };

            Constants.db.Insert(wd);
            lastPoint++;
        }

        ChangePauseStatus();
    }
}
