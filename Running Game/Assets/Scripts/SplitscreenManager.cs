using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitscreenManager : MonoBehaviour {

    void Start() {
        //SetSplitscreen(4);
    }

    public void SetSplitscreen(int numberOfScreens) {
        numberOfScreens = Mathf.Clamp(numberOfScreens, 0, FindObjectsOfType<FollowTrack>().Length);

        if (GameObject.Find("Player_1")) {
            Camera cam = GameObject.Find("Player_1").GetComponentInChildren<Camera>();
            if (cam) {
                cam.rect = new Rect(0, 0, 1f / (numberOfScreens), 1);
            }
        }
        if (GameObject.Find("Player_2")) {
            Camera cam = GameObject.Find("Player_2").GetComponentInChildren<Camera>();
            if (cam) {
                cam.rect = new Rect(1f / numberOfScreens, 0, 1f / numberOfScreens, 1);
            }
        }
        if (GameObject.Find("Player_3")) {
            Camera cam = GameObject.Find("Player_3").GetComponentInChildren<Camera>();
            if (cam) {
                cam.rect = new Rect((1f / numberOfScreens) * 2, 0, 1f / numberOfScreens, 1);
            }
        }
        if (GameObject.Find("Player_4")) {
            Camera cam = GameObject.Find("Player_4").GetComponentInChildren<Camera>();
            if (cam) {
                cam.rect = new Rect((1f / numberOfScreens) * 3, 0, 1f / numberOfScreens, 1);
            }
        }
    }
}
