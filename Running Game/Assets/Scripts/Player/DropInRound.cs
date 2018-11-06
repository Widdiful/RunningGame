using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropInRound : MonoBehaviour
{
    const int maxplayers = 4;
    bool gameStarted;
    const string playerJoin = "Join_";
    private Dictionary<string, bool> playersJoined;
    private GameData gameData;
    private float camdistance = 10;
    public float proximity = 1;
    private float screenX;
    private float screenY;
    private float margin = 0.1f;
    private Camera cam;
    private Canvas spawnCanvas;
    private float playerScaleX;
    private float playerScaleY;
    public SplineCurve startingSpline;
	public bool Playerselect { get; set; }


    // Use this for initialization
    void Start()
    {
        gameData = gameObject.GetComponent<GameData>();
        playersJoined = new Dictionary<string, bool>();
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerScaleX = Resources.Load<GameObject>("Player").gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.x;
        playerScaleY = Resources.Load<GameObject>("Player").gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Playerselect)
        {
            CheckDropIns();
        }

        if (Input.GetKey("joystick 1 button 9") || Input.GetKey(KeyCode.Return) && gameData.players.Count > 0)
        {
            gameStarted = true;
            FindObjectOfType<SplitscreenManager>().SetSplitscreen(gameData.players.Count);
        }

        if (gameStarted)
        {
            SetStart();
        }
    }

    //Set the start position of all players and attach main camera to player one to follow. finally destroy this script to save resources and update cycles
    private void SetStart()
    {        
        var players = gameData.players;
        for (int i = 0; i < gameData.players.Count; i++)
        {
            players[i].transform.position = new Vector3(0, players[i].transform.lossyScale.y * 2, players[i].transform.lossyScale.z * i + proximity * i);
            players[i].transform.rotation = Quaternion.identity;
            players[i].GetComponent<FollowTrack>().enabled = true;
            //gameData.cam.transform.parent = gameData.players[0].transform;
            players[i].GetComponent<FollowTrack>().InitialiseRunner(startingSpline, (float)i / (float)gameData.players.Count);

        }

        Destroy(GetComponent<DropInRound>());
    }

    //Checks if any controller pressed any key and adds the corresponding controller to the 
    //player joining dictionary
    private void CheckDropIns()
    {

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            try
            {
                if (!playersJoined.ContainsKey(playerJoin + (i + 1)))
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (j != 8 && Input.GetKeyUp("joystick " + (i + 1) + " button " + j))
                        {
                            SpawnPlayer(i + 1);
                        }
                    }
                }
            }catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        //TESTING
        if (Input.GetKeyUp(KeyCode.Alpha1) && !playersJoined.ContainsKey(playerJoin  + 1)) { SpawnPlayer(1); }
        if (Input.GetKeyUp(KeyCode.Alpha2) && !playersJoined.ContainsKey(playerJoin + 2)) { SpawnPlayer(2); }
        if (Input.GetKeyUp(KeyCode.Alpha3) && !playersJoined.ContainsKey(playerJoin + 3)) { SpawnPlayer(3); }
        if (Input.GetKeyUp(KeyCode.Alpha4) && !playersJoined.ContainsKey(playerJoin + 4)) { SpawnPlayer(4); }

        for (int i = 0; i < gameData.players.Count; i++)
        {
            UpdateDropInPosition(gameData.players, i);
        }
    }

    //Spawns player and adds him to the player list in gamedata 
    private void SpawnPlayer(int pNumber)
    {
        playersJoined.Add(playerJoin + pNumber, true);
        Debug.Log("Player " + pNumber + " joined");
        GameObject tmpPlayer = Instantiate(Resources.Load<GameObject>("Player") as GameObject);		
        tmpPlayer.GetComponent<Posing>().ID = pNumber;
		
        tmpPlayer.name = "Player_" + pNumber;
        tmpPlayer.gameObject.transform.LookAt(-cam.transform.position);

        UpdateDropInPosition(tmpPlayer, pNumber);

        gameData.players.Add(tmpPlayer);
		Debug.Log(tmpPlayer.GetComponent<Posing>().ID);
    }

    private void UpdateDropInPosition(List<GameObject> ps, int number)
    {
        UpdateDropInPosition(ps[number], number);
    }

    private void UpdateDropInPosition(GameObject player, int number)
    {

        player.gameObject.transform.position = cam.ViewportToWorldPoint(new Vector3((1.0f / maxplayers) * number + (playerScaleX / camdistance), margin + playerScaleY * 2 / camdistance, camdistance));
    }
}
