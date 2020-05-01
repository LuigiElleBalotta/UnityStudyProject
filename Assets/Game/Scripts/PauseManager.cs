using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Database.Entities;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool isPaused;
    public GameObject pnlPause;

    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyUp(KeyCode.Escape))
        {
            //attivare o disattivare la pausa
            ChangePauseStatus();
            
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
}
