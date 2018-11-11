using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour {

    public Transform target;
    public Transform secondTarget;
    private Vector3 pointTarget;

    void Update() {
        if (target) {
            pointTarget = target.position;
            if (secondTarget) {
                pointTarget = target.position + (secondTarget.position - target.position) / 2;
            }
            transform.LookAt(pointTarget);
        }
    }
}
