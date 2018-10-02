using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrack : MonoBehaviour {

    public SplineCurve spline;
    public float speed;
    public float progress;
    public bool lookForward;
    public float speedIncreaseCountdown;
    public float speedIncreaseRate;
    public float horizontalAdjust;
    public float moveSpeed;

    private int currentCurve = 0;
    private Vector3 targetPosition;

    private void Start()
    {
        MoveTarget(1);
    }

    private void Update() {
        if (speedIncreaseCountdown <= 0) {
            speed += Time.deltaTime * speedIncreaseRate;
        }
        else {
            speedIncreaseCountdown -= Time.deltaTime;
        }
        MoveTowardsTarget();

        if (true) { 
            MoveTarget(speed);
        }

    }

    private void MoveTarget(float amount) {
        progress += (Time.deltaTime / spline.GetLengthOfCurve(currentCurve / 3)) * amount;
        if (progress >= 1f) {
            if (currentCurve / 3 < spline.CurveCount - 1) {
                currentCurve += 3;
                progress -= 1f;
            }
            else {
                if (spline.Loop) {
                    currentCurve = 0;
                    progress -= 1f;
                }
                else {
                    currentCurve = spline.CurveCount;
                    progress = 1f;
                }
            }
        }
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
        Vector3 position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);
        transform.localPosition = position;

        if (lookForward)
        {
            transform.LookAt(targetPosition);
        }
        transform.Translate(new Vector3(horizontalAdjust, 0, 0), Space.Self);
    }

    public void IncreaseSpeed(float amount) {
        speed += amount;
    }
}
