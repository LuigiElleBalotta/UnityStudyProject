using System.Collections;
using System.Collections.Generic;
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
}
