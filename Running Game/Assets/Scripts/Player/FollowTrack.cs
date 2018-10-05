using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrack : MonoBehaviour {

    public SplineCurve spline;
    public SplineCurve secondSpline;
    public float stepSize;
    public float stepDistance;
    public int currentCurve = 0;
    public float progress;
    public bool lookForward;
    public float speedIncreaseCountdown;
    public float speedIncreaseRate;
    public float horizontalAdjust;
    public float verticalAdjust;
    public float moveSpeed;

    public Vector3 targetPosition;

    private void Start()
    {
        MoveTarget(1);
    }

    private void Update() {
        stepDistance = 1 + Mathf.Abs(horizontalAdjust);
        if (speedIncreaseCountdown <= 0) {
            stepSize += Time.deltaTime * speedIncreaseRate;
        }
        else {
            speedIncreaseCountdown -= Time.deltaTime;
        }
        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, targetPosition) <= stepDistance) { 
            MoveTarget(stepSize);
        }

        if (Input.GetKeyDown("a"))
        {
            ChangeSpline(secondSpline);
        }
    }

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
        Vector3 tempTargetPosition = transform.InverseTransformPoint(targetPosition);
        tempTargetPosition.x += horizontalAdjust;
        tempTargetPosition.y += verticalAdjust;
        tempTargetPosition = transform.TransformPoint(tempTargetPosition);
        Vector3 position = Vector3.MoveTowards(transform.position, tempTargetPosition, moveSpeed);

        if (lookForward)
        {
            if (tempTargetPosition - transform.position != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tempTargetPosition - transform.position), 0.1f);
            }
        }
        transform.localPosition = position;
        Debug.DrawLine(transform.position, tempTargetPosition);
        Debug.DrawLine(tempTargetPosition, targetPosition);
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

    }
}
