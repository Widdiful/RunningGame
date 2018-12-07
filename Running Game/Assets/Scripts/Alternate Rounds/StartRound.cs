using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartRound : MonoBehaviour {

    private GameManager gm;
    

    // Use this for initialization
    void Start()
    {
       gm = FindObjectOfType<GameManager>();
        GameObject.Find("MainMenu").GetComponent<Menu>().ToggleActive();
        StartRoundNow();
    }

    void StartRoundNow()
    {
        if (gm)
        {
            if (gm.gameStarted || gm.gameStartedSolo)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                return;
            }
        }

        GameObject.Find("GameUpdates").GetComponent<DropInRound>().Playerselect = true;

        foreach (FollowTrack runner in FindObjectsOfType<FollowTrack>())
        {
            Destroy(runner.gameObject);
        }

        SpectatorCamera spectator = FindObjectOfType<SpectatorCamera>();

        spectator.transform.position = new Vector3(0, 10, -10);
        spectator.transform.rotation = Quaternion.Euler(30, 0, 0);
        spectator.enabled = false;
    }
}
