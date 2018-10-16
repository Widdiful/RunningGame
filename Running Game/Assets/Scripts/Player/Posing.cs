using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posing : MonoBehaviour
{

    public int ID;
    private const float threshold = 0.6f;
    private Animator animator;
    private RaycastHit hit;
    private const float checkRange = 20.0f;
    private GameObject currentWall;    
    private string leftPose = "";
    private string rightPose = "";
    private int startPose;
    private FollowTrack track;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        //ID = 1;
        startPose = Random.Range(0, 9);
        R_PoseUp();
        L_PoseUp();
        track = GetComponent<FollowTrack>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPoses();
    }

    private void CheckPoses()
    {
        //if(Input.GetAxis())
			//Debug.Log(ID);
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") > threshold || /* TESTING */ Input.GetKey(KeyCode.D)) {    Debug.Log("Player " + ID + " turns right");     TurnRight(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickHori") < -threshold || /* TESTING */ Input.GetKey(KeyCode.A)) {    Debug.Log("Player " + ID + " left arm out");   L_PoseOut(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") < -threshold || /* TESTING */ Input.GetKey(KeyCode.W)) {    Debug.Log("Player " + ID + " left arm up")  ;  L_PoseUp(); }
        if (Input.GetAxis("Joy" + ID + "_LeftStickVert") > threshold || /* TESTING */ Input.GetKey(KeyCode.S)) {     Debug.Log("Player " + ID + " left arm down");  L_PoseDown(); }

        if (Input.GetAxis("Joy" + ID + "_RightStickHori") < -threshold || /* TESTING */ Input.GetKey(KeyCode.LeftArrow)) { Debug.Log("Player " + ID + " turns left");     TurnLeft(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickHori") > threshold || /* TESTING */ Input.GetKey(KeyCode.RightArrow)) { Debug.Log("Player " + ID + " right arm out");  R_PoseOut(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") < -threshold || /* TESTING */ Input.GetKey(KeyCode.UpArrow)) {    Debug.Log("Player " + ID + " right arm up");  R_PoseUp(); }
        if (Input.GetAxis("Joy" + ID + "_RightStickVert") > threshold || /* TESTING */ Input.GetKey(KeyCode.DownArrow)) {  Debug.Log("Player " + ID + " right arm down"); R_PoseDown(); }
    }

    private void CheckNextWall()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * checkRange, out hit))
        {
            Debug.Log("Hit Wall");
            currentWall = hit.transform.gameObject;
            Debug.Log(currentWall.name);
        }
    }

    private void TurnLeft()
    {
        if (track) {
            track.TurnLeft();
        }
    }

    private void TurnRight()
    {
        if (track) {
            track.TurnRight();
        }
    }

    private void R_PoseOut()
    {
        animator.Play("RightArmOut");
        rightPose = "RO";
    }

    private void R_PoseDown()
    {
        animator.Play("RightArmDown");
        rightPose = "RD";
    }

    private void R_PoseUp()
    {
        animator.Play("RightArmUp");
        rightPose = "RU";
    }

    private void L_PoseOut()
    {
       // animator.SetInteger("CurrentLeftPos", 0);
       animator.Play("LeftArmOut");
        leftPose = "LO";
    }

    private void L_PoseUp()
    {
       // animator.SetInteger("CurrentLeftPos", 1);
       animator.Play("LeftArmUp");
        leftPose = "LU";
    }

    private void L_PoseDown()
    {
      // animator.SetInteger("CurrentLeftPos", 2);
        animator.Play("LeftArmDown");
        leftPose = "LD";
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if(collision.transform.gameObject.CompareTag("Wall"))
    //    //{
    //    // //   if(collision.transform.gameObject.name == "Wall")
    //    //    for (int i = 0; i < poses.Length; i++)
    //    //    {
    //    //        if(collision.transform.gameObject.name.Contains(poses[i]))
    //    //        {

    //    //        }

    //    //    }
    //    //}
    //    //Debug.Log("test");
    //}

    public string GetPose()
    {
        return leftPose + "_" + rightPose;
    }
}
