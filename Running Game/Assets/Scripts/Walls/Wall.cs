using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public SplineCurve attachedSpline;
    [Range(0, 1)]
    public float positionOnSpline;

    void Start() {
        positionOnSpline = attachedSpline.GetPositionOnSpline(transform.position);
    }
	
	void Update () {
        transform.position = attachedSpline.GetPoint(positionOnSpline);
        transform.LookAt(transform.position + attachedSpline.GetDirection(positionOnSpline));
	}
}
