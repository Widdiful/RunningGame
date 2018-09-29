using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInRound : MonoBehaviour
{
    bool gameStarted;
    const string playerJoin = "Join_";
    Dictionary<string, bool> playersJoined;

    // Use this for initialization
    void Start()
    {
        playersJoined = new Dictionary<string, bool>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            CheckDropIns();
        }     
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
                    if (Input.GetKeyUp("joystick " + (i + 1) + " button " + j))
                    {
                        playersJoined.Add(playerJoin + (i + 1), true);
                        Debug.Log("Player " + (i + 1) + " joined");
                    }
                }
            }
        }
    }
}
