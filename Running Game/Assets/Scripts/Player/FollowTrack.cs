using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrack : MonoBehaviour {

    public SplineCurve spline;
    public float stepSize;
    public float stepDistance;
    public int currentCurve = 0;
    public float progress;
    public bool lookForward;
    public float speedIncreaseCountdown;
    public float speedIncreaseRate;
    public float maximumSpeed;
    public float horizontalAdjust;
    public float verticalAdjust;
    public float moveSpeed;
    public float distanceFromNewPath;

    public Vector3 targetPosition;
    private Vector3 adjustedTargetPosition;
    public SplineCurve leftSpline;
    public SplineCurve rightSpline;
    private SubPath[] subPaths;

    private void Start()
    {
        MoveTarget(1);
        subPaths = FindObjectsOfType<SubPath>();
    }

    private void Update() {
        if (speedIncreaseCountdown <= 0) {
            moveSpeed = Mathf.Clamp(moveSpeed + Time.deltaTime * speedIncreaseRate, 0, maximumSpeed);
        }
        else {
            speedIncreaseCountdown -= Time.deltaTime;
        }
        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, adjustedTargetPosition) <= stepDistance) { 
            MoveTarget(stepSize);
        }

        if (Input.GetKeyDown("z"))
        {
            if (leftSpline && leftSpline != spline) ChangeSpline(leftSpline);
        }
        if (Input.GetKeyDown("x")) {
            if (rightSpline && rightSpline != spline) ChangeSpline(rightSpline);
        }

        // Detect nearby splines
        leftSpline = null;
        rightSpline = null;
        foreach(SubPath path in subPaths) {
            if (Vector3.Distance(adjustedTargetPosition, path.transform.position) <= distanceFromNewPath &&
                path.startingSpline == spline && Vector3.Dot(path.transform.position, transform.forward) > 0) {
                if (path.direction == SubPath.BranchDirection.Right) {
                    if (rightSpline) {
                        if (Vector3.Distance(adjustedTargetPosition, path.transform.position) <
                            Vector3.Distance(adjustedTargetPosition, rightSpline.transform.position)) {
                            rightSpline = path.GetComponent<SplineCurve>();
                        }
                    }
                    else {
                        rightSpline = path.GetComponent<SplineCurve>();
                    }
                }
                else {
                    if (leftSpline) {
                        if (Vector3.Distance(adjustedTargetPosition, path.transform.position) <
                            Vector3.Distance(adjustedTargetPosition, leftSpline.transform.position)) {
                            leftSpline = path.GetComponent<SplineCurve>();
                        }
                    }
                    else if (path.direction == SubPath.BranchDirection.Left) {
                        leftSpline = path.GetComponent<SplineCurve>();
                    }
                }
            }
        }
    }

    // Moves the target point along the spline
    private void MoveTarget(float amount) {
        if (progress >= 1f)
        {
            if (currentCurve / 3 < spline.CurveCount - 1)
            {
                currentCurve += 3;
                progress -= 1f;
            }
            else
            {
                if (spline.Loop)
                {
                    currentCurve = 0;
                    progress -= 1f;
                }
                else
                {
                    if (spline.gameObject.GetComponent<SubPath>())
                    {
                        spline.gameObject.GetComponent<SubPath>().LeaveSpline(this);
                    }
                    else
                    {
                        currentCurve = spline.CurveCount;
                        progress = 1f;
                    }
                }
            }
        }
        progress += (Time.deltaTime / spline.GetLengthOfCurve(currentCurve / 3)) * amount;
        targetPosition = spline.GetPointOnCurve(currentCurve, progress);

        /*
        Vector3 position = spline.GetPointOnCurve(currentCurve, progress);
        transform.localPosition = position;

        if (lookForward) {
            transform.LookAt(position + spline.GetVelocityOnCurve(currentCurve, progress).normalized);
        }
        transform.Translate(new Vector3(horizontalAdjust, 0, 0), Space.Self);
        */
    }

    private void MoveTowardsTarget()
    {
        adjustedTargetPosition = transform.InverseTransformPoint(targetPosition);
        adjustedTargetPosition.x += horizontalAdjust;
        adjustedTargetPosition.y += verticalAdjust;
        adjustedTargetPosition = transform.TransformPoint(adjustedTargetPosition);
        Vector3 position = Vector3.MoveTowards(transform.position, adjustedTargetPosition, moveSpeed);

        if (lookForward)
        {
            if (adjustedTargetPosition - transform.position != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(adjustedTargetPosition - transform.position), 0.1f);
            }
        }
        transform.localPosition = position;
        Debug.DrawLine(transform.position, adjustedTargetPosition);
        Debug.DrawLine(adjustedTargetPosition, targetPosition);
    }

    public void IncreaseSpeed(float amount) {
        stepSize += amount;
    }

    public void ChangeSpline(SplineCurve newSpline)
    {
        spline = newSpline;
        progress = 0;
        currentCurve = 0;
    }

    public void ChangeSpline(SplineCurve newSpline, Vector3 position)
    {
        spline = newSpline;
        leftSpline = null;
        rightSpline = null;
    }
}
