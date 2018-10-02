using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posing : MonoBehaviour {

    public int ID { get; set; }
    private const float threshold = 0.6f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckPoses();
	}

    private void CheckPoses()
    {
        //if(Input.GetAxis())
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") < -threshold) { Debug.Log("Player " + ID + " left arm out"); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") > threshold) { Debug.Log("Player " + ID + " left arm in"); TurnRight(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") < -threshold) { Debug.Log("Player " + ID + " left arm up"); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") > threshold) { Debug.Log("Player " + ID + " left arm down"); }

        if (Input.GetAxis("Joy" + ID + "_RightStickHori") < -threshold) { Debug.Log("Player " + ID + " right arm in"); TurnLeft(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickHori") > threshold) { Debug.Log("Player " + ID + " right arm out"); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") < -threshold) { Debug.Log("Player " + ID + " right arm up"); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") > threshold) { Debug.Log("Player " + ID + " right arm down"); }
    }

    private void TurnLeft()
    {

    }

    private void TurnRight()
    {

    }

    private void R_PoseOut()
    {

    }

    private void R_PoseDown()
    {

    }

    private void R_PoseUp()
    {

    }

    private void L_PoseOut()
    {

    }

    private void L_PoseDown()
    {

    }

    private void L_PoseUp()
    {

    }


}
