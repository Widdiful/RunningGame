using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float slowestSpeed;
    public float maximumSpeedDifference;
    public float globalSpeedCap;
    public Vector3 lastPosition;

    void Update() {
        slowestSpeed = 0;
        if (FindObjectsOfType<FollowTrack>().Length >= 2) {
            foreach (FollowTrack track in FindObjectsOfType<FollowTrack>()) {
                if (slowestSpeed == 0 || track.moveSpeed < slowestSpeed) {
                    slowestSpeed = track.moveSpeed;
                }
            }
        }
        if (slowestSpeed == 0) {
            globalSpeedCap = 10;
        }
        else {
            globalSpeedCap = slowestSpeed + maximumSpeedDifference;
        }
    }
}
