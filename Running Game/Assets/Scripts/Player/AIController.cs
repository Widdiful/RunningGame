using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    [Range(0, 1)]
    public float accuracy;
    [Range(0, 1)]
    public float chanceToChangePath;
    public float distanceToPose = 20;

    private FollowTrack track;
    private Posing pose;
    public bool posing;
    public bool raycastWall;
    private float poseTimer = 1.0f;
    private float overtakeTimer = 1.0f;
    public bool canTurn;

    private string[] poses = new[] { "LD_RD", "LD_RO", "LD_RU", "LO_RD", "LO_RO", "LO_RU", "LU_RD", "LU_RO", "LU_RU" };
    private string currentRandom = "";
    private bool overtaking;

    private void Start() {
        track = GetComponent<FollowTrack>();
        pose = GetComponent<Posing>();
    }

    private void Update() {
        Wall nextWall = new Wall();
        raycastWall = false;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 10);
        foreach(RaycastHit hit in hits) {
            if (hit.transform.GetComponent<Wall>()) {
                nextWall = hit.transform.GetComponent<Wall>();
                raycastWall = true;
            }
            if (hit.transform.GetComponent<FollowTrack>()) {
                overtaking = true;
            }
        }

        if (overtaking) {
            track.TurnRight();
        }

        if (nextWall == null) {
            nextWall = track.nextWall;
        }

        if (track) {
            if (nextWall) {
                if (track.distanceToNextWall > distanceToPose && !raycastWall) {
                    posing = false;
                }

                if (track.distanceToNextWall <= distanceToPose || raycastWall) {
                    if (nextWall.broken && !posing) {
                        if (Random.Range(0.0f, 1.0f) <= accuracy) {
                            pose.Pose(nextWall.wallPose);
                            posing = true;
                        }
                        else {
                            string newPose = poses[Random.Range(0, poses.Length - 1)];
                            while (newPose == currentRandom) {
                                newPose = poses[Random.Range(0, poses.Length - 1)];
                            }
                            currentRandom = newPose;
                            pose.Pose(newPose);
                            posing = true;
                        }
                    }
                    else if (!posing) {
                        string newPose = poses[Random.Range(0, poses.Length - 1)];
                        while (newPose == currentRandom) {
                            newPose = poses[Random.Range(0, poses.Length - 1)];
                        }
                        currentRandom = newPose;
                        pose.Pose(newPose);
                        posing = true;
                    }
                }
            }

            if ((track.leftSpline || track.rightSpline)) {
                 if ((Random.Range(0.0f, 1.0f) <= chanceToChangePath) && canTurn){
                    if (track.leftSpline)
                        track.TurnLeft();
                    if (track.rightSpline)
                        track.TurnRight();
                }
                canTurn = false;
            }
            else if (!track.nextSpline && !track.leftSpline && !track.rightSpline){
                canTurn = true;
            }
        }

        //if (posing) {
        //    poseTimer -= Time.deltaTime;
        //    if (poseTimer <= 0) {
        //        poseTimer = 1.0f;
        //        posing = false;
        //    }
        //}

        if (overtaking) {
            overtakeTimer -= Time.deltaTime;
            if (overtakeTimer <= 0) {
                overtakeTimer = 1.0f;
                overtaking = false;
            }
        }
    }
}
