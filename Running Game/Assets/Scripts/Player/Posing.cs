using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posing : MonoBehaviour {

    public int ID { get; set; }
    private const float threshold = 0.6f;
    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        ID = 1;
	}
	
	// Update is called once per frame
	void Update () {
        CheckPoses();
	}

    private void CheckPoses()
    {
        //if(Input.GetAxis())
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") > threshold || /* TESTING */ Input.GetKey(KeyCode.D)) { Debug.Log("Player " + ID + " turns right"); TurnRight(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") < -threshold || /* TESTING */ Input.GetKey(KeyCode.A)) { Debug.Log("Player " + ID + " left arm out");  L_PoseOut(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") < -threshold || /* TESTING */ Input.GetKey(KeyCode.W)) { Debug.Log("Player " + ID + " left arm up");L_PoseUp(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") > threshold || /* TESTING */ Input.GetKey(KeyCode.S)) { Debug.Log("Player " + ID + " left arm down"); L_PoseDown(); }

        if (Input.GetAxis("Joy" + ID + "_RightStickHori") < -threshold || /* TESTING */ Input.GetKey(KeyCode.LeftArrow)) { Debug.Log("Player " + ID + " turns left"); TurnLeft(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickHori") > threshold || /* TESTING */ Input.GetKey(KeyCode.RightArrow)) { Debug.Log("Player " + ID + " right arm out"); R_PoseOut(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") < -threshold || /* TESTING */ Input.GetKey(KeyCode.UpArrow)) { Debug.Log("Player " + ID + " right arm up"); R_PoseUp(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") > threshold || /* TESTING */ Input.GetKey(KeyCode.DownArrow)) { Debug.Log("Player " + ID + " right arm down"); R_PoseDown(); }
    }

    private void TurnLeft()
    {

    }

    private void TurnRight()
    {

    }

    private void R_PoseOut()
    {
        animator.Play("RightArmOut");
    }

    private void R_PoseDown()
    {
        animator.Play("RightArmDown");
    }

    private void R_PoseUp()
    {
        animator.Play("RightArmUp");
    }

    private void L_PoseOut()
    {
      // animator.SetInteger("CurrentLeftPos", 0);
        animator.Play("LeftArmOut");
    }

    private void L_PoseUp()
    {
       // animator.SetInteger("CurrentLeftPos", 1);
        animator.Play("LeftArmUp");
    }

    private void L_PoseDown()
    {
       // animator.SetInteger("CurrentLeftPos", 2);
        animator.Play("LeftArmDown");
    }
}
