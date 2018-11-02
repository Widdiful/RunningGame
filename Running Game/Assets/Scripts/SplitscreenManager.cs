using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitscreenManager : MonoBehaviour {

    void Start() {
        SetSplitscreen(4);
    }

    public void SetSplitscreen(int numberOfScreens) {
        numberOfScreens = Mathf.Clamp(numberOfScreens, 0, FindObjectsOfType<FollowTrack>().Length);
        if (numberOfScreens <= 3) {
            if (GameObject.Find("Player_1")) {
                foreach(Camera cam in GameObject.Find("Player_1").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0, 1f / (numberOfScreens), 1);
                }
            }
            if (GameObject.Find("Player_2")) {
                foreach (Camera cam in GameObject.Find("Player_2").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(1f / numberOfScreens, 0, 1f / numberOfScreens, 1);
                }
            }
            if (GameObject.Find("Player_3")) {
                foreach (Camera cam in GameObject.Find("Player_3").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect((1f / numberOfScreens) * 2, 0, 1f / numberOfScreens, 1);
                }
            }
        }
        else {
            if (GameObject.Find("Player_1")) {
                foreach (Camera cam in GameObject.Find("Player_1").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
            }
            if (GameObject.Find("Player_2")) {
                foreach (Camera cam in GameObject.Find("Player_2").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
            if (GameObject.Find("Player_3")) {
                foreach (Camera cam in GameObject.Find("Player_3").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                }
            }
            if (GameObject.Find("Player_4")) {
                foreach (Camera cam in GameObject.Find("Player_4").GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                }
            }
        }
    }
}
