using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitscreenManager : MonoBehaviour {

    void Start() {
        SetSplitscreen(4);
    }

    public void SetSplitscreen(int numberOfScreens) {
        numberOfScreens = Mathf.Clamp(numberOfScreens, 0, FindObjectsOfType<FollowTrack>().Length);

        FollowTrack[] players = FindObjectsOfType<FollowTrack>();
        GameObject p1 = new GameObject();
        GameObject p2 = new GameObject();
        GameObject p3 = new GameObject();
        GameObject p4 = new GameObject();
        foreach (FollowTrack player in players) {
            switch (player.player_id) {
                case 0:
                    p1 = player.gameObject;
                    break;
                case 1:
                    p2 = player.gameObject;
                    break;
                case 2:
                    p3 = player.gameObject;
                    break;
                case 3:
                    p4 = player.gameObject;
                    break;
            }
        }

        if (numberOfScreens <= 2) {
            if (p1) {
                foreach(Camera cam in p1.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0, 1f / (numberOfScreens), 1);
                }
            }
            if (p2) {
                foreach (Camera cam in p2.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(1f / numberOfScreens, 0, 1f / numberOfScreens, 1);
                }
            }
        }
        else {
            if (p1) {
                foreach (Camera cam in p1.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                if (p1.GetComponentInChildren<CanvasScaler>()){
                    p1.GetComponentInChildren<CanvasScaler>().referenceResolution *= 1.25f;
                }
            }
            if (p2) {
                foreach (Camera cam in p2.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                if (p2.GetComponentInChildren<CanvasScaler>()) {
                    p2.GetComponentInChildren<CanvasScaler>().referenceResolution *= 1.25f;
                }
            }
            if (p3) {
                foreach (Camera cam in p3.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                }
                if (p3.GetComponentInChildren<CanvasScaler>()) {
                    p3.GetComponentInChildren<CanvasScaler>().referenceResolution *= 1.25f;
                }
            }
            if (p4) {
                foreach (Camera cam in p4.GetComponentsInChildren<Camera>())
                {
                    cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                }
                if (p4.GetComponentInChildren<CanvasScaler>()) {
                    p4.GetComponentInChildren<CanvasScaler>().referenceResolution *= 1.25f;
                }
            }
        }
    }
}
