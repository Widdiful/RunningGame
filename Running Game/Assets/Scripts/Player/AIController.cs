using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    [Range(0, 1)]
    public float accuracy;
    public float distanceToPose;

    private FollowTrack track;
    private Posing pose;
    public bool posing;
    public bool raycastWall;
    private float poseTimer = 1.0f;

    private string[] poses = new[] { "LD_RD", "LD_RO", "LD_RU", "LO_RD", "LO_RO", "LO_RU", "LU_RD", "LU_RO", "LU_RU" };

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
        }
        if (nextWall == null) {
            nextWall = track.nextWall;
        }

        if (track) {
            if (nextWall) {
                if (track.distanceToNextWall <= distanceToPose || raycastWall) {
                    if (nextWall.broken) {
                        if (Random.Range(0.0f, 1.0f - accuracy) == 0) {
                            pose.Pose(nextWall.wallPose);
                            posing = true;
                        }
                        else if (!posing) {
                            pose.Pose(poses[Random.Range(0, poses.Length - 1)]);
                            posing = true;
                        }
                    }
                    else if (!posing) {
                        pose.Pose(poses[Random.Range(0, poses.Length - 1)]);
                        posing = true;
                    }
                }
            }
        }

        if (posing) {
            poseTimer -= Time.deltaTime;
            if (poseTimer <= 0) {
                poseTimer = 1.0f;
                posing = false;
            }
        }
    }
}
