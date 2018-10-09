using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInRound : MonoBehaviour
{
    const int maxplayers = 4;
    bool gameStarted;
    const string playerJoin = "Join_";
    private Dictionary<string, bool> playersJoined;
    GameData gameData;
    private float camdistance = 10;
    public float proximity = 1;
    private float screenX;
    private float screenY;
    private float margin = 0.1f;
    private Camera cam;
    private Canvas spawnCanvas;


    // Use this for initialization
    void Start()
    {
        gameData = gameObject.GetComponent<GameData>();
        playersJoined = new Dictionary<string, bool>();
        cam = GameObject.Find("Player1Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            CheckDropIns();
        }

        if (Input.GetKey("joystick 1 button 9") || Input.GetKey(KeyCode.Return) && playersJoined.Count > 0)
        {
            gameStarted = true;
        }

        if (gameStarted)
        {
            SetStart();
        }
    }

    private void SetStart()
    {
        var players = gameData.players;
        for (int i = 0; i < gameData.players.Count; i++)
        {
            players[i].transform.position = new Vector3(0, players[i].transform.lossyScale.y, players[i].transform.lossyScale.z * i + proximity * i);
            players[i].transform.rotation = Quaternion.identity;
            players[i].GetComponent<FollowTrack>().enabled = true;
        }

        Destroy(GetComponent <DropInRound>());
        //Debug.Log("round started");
    }

    //Checks if any controller pressed any key and adds the corresponding controller to the 
    //player joining dictionary
    private void CheckDropIns()
    {

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (!playersJoined.ContainsKey(playerJoin + (i + 1)))
            {
                for (int j = 0; j < 20; j++)
                {
                    if (j != 9 && Input.GetKeyUp("joystick " + (i + 1) + " button " + j))
                    {
                        SpawnPlayer(i + 1);
                    }
                }
            }
        }

        //TESTING
        if (Input.GetKeyUp(KeyCode.Alpha1)) { SpawnPlayer(1); }
        if (Input.GetKeyUp(KeyCode.Alpha2)) { SpawnPlayer(2); }
        if (Input.GetKeyUp(KeyCode.Alpha3)) { SpawnPlayer(3); }
        if (Input.GetKeyUp(KeyCode.Alpha4)) { SpawnPlayer(4); }

        for (int i = 0; i < gameData.players.Count; i++)
        {
            UpdateDropInPosition(gameData.players,i);
        }
    }

    private void SpawnPlayer(int pNumber)
    {
        playersJoined.Add(playerJoin + pNumber, true);
        Debug.Log("Player " + pNumber + " joined");
        GameObject tmpPlayer = Instantiate(Resources.Load<GameObject>("Player") as GameObject);
        tmpPlayer.GetComponent<Posing>().ID = pNumber;
        tmpPlayer.name = "Player_" + pNumber;
        tmpPlayer.gameObject.transform.LookAt(cam.transform.position);
        //float scaleX = tmpPlayer.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.x;
        //float scaleY = tmpPlayer.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.y;
        ////Debug.Log(scaleX);

        //tmpPlayer.gameObject.transform.position = cam.ViewportToWorldPoint(new Vector3((1.0f/maxplayers ) * (pNumber - 1) + (scaleX / camdistance),margin + scaleY * 2 / camdistance, camdistance));
        UpdateDropInPosition(tmpPlayer, pNumber);

        gameData.players.Add(tmpPlayer);
    }

    private void UpdateDropInPosition(List<GameObject> ps, int number)
    {
        //float scaleX = ps[number].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.x;
        //float scaleY = ps[number].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.y;
        //ps[number].gameObject.transform.position = cam.ViewportToWorldPoint(new Vector3((1.0f / maxplayers) * (number - 1) + (scaleX / camdistance), margin + scaleY * 2 / camdistance, camdistance));

        UpdateDropInPosition(ps[number], number);
    }

    private void UpdateDropInPosition(GameObject player, int number)
    {
        float scaleX = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.x;
        float scaleY = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.y;
        player.gameObject.transform.position = cam.ViewportToWorldPoint(new Vector3((1.0f / maxplayers) * number + (scaleX / camdistance), margin + scaleY * 2 / camdistance, camdistance));
    }
}
