using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPath : MonoBehaviour {

    [Tooltip("This is the spline that is connected to the start of this spline")]
    public SplineCurve startingSpline;
    [Tooltip("This is the spline that is connected to the end of this spline")]
    public SplineCurve exitSpline;

    public enum BranchDirection { Left, Right, None };
    public BranchDirection direction;

    private SplineCurve spline;

    private void Start()
    {
        spline = GetComponent<SplineCurve>();
    }

    public void LeaveSpline(FollowTrack followInfo)
    {
        followInfo.spline = exitSpline;
        float newProgress = exitSpline.GetNearestPointFromVector(spline.GetPoint(1));
        followInfo.currentCurve = Mathf.FloorToInt(newProgress) * 3;
        followInfo.progress = newProgress - Mathf.FloorToInt(newProgress);
        followInfo.targetPosition = exitSpline.GetPointOnCurve(followInfo.currentCurve, followInfo.progress);
    }
}
