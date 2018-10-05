using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPath : MonoBehaviour {

    public SplineCurve connectedSpline;
	private SplineCurve spline;

    private void Start()
    {
        spline = GetComponent<SplineCurve>();
    }

    public void LeaveSpline(FollowTrack followInfo)
    {
        followInfo.spline = connectedSpline;
        float newProgress = connectedSpline.GetNearestPointFromVector(spline.GetPoint(1));
        followInfo.currentCurve = Mathf.FloorToInt(newProgress) * 3;
        followInfo.progress = newProgress - Mathf.FloorToInt(newProgress);
        print(newProgress);
        print(followInfo.currentCurve);
        print(followInfo.progress);
        followInfo.targetPosition = connectedSpline.GetPointOnCurve(followInfo.currentCurve, followInfo.progress);
    }
}
