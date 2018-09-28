using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInRound : MonoBehaviour {


    int i = 1;
    // Use this for initialization
    void Start()
    {
        //        string name = Input.GetJoystickNames()[0];
        

    }
        // Update is called once per frame
        void Update () {
        while (i < 3)
        {
            if (Input.GetButtonUp("Joy" + i + "B"))
            {

                //Debug.Log(Input.GetJoystickNames()[i] + " is pressed");
            }

            i++;

 
        }

        if (Input.GetButtonUp("Joy3B"))
            Debug.Log(Input.GetJoystickNames().ToString()[2]);
        if (Input.GetButtonUp("Joy1B"))
            Debug.Log(Input.GetJoystickNames().ToString()[0]);
        
        i = 1;
        //if (Input.GetButtonDown("Test"))
        //    Debug.Log("Button pressed");
        //if()
    }
}
