using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrack : MonoBehaviour {

    public SplineCurve spline;
    public float duration;
    public float progress;
    public bool lookForward;

    private void Update() {
        progress += Time.deltaTime / duration;
        if (progress > 1f) {
            if (spline.Loop) {
                progress = 0f;
            }
            else {
                progress = 1f;
            }
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward) {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}
