using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowTrack : MonoBehaviour
{

    public SplineCurve spline;
    public float stepSize;
    public float stepDistance;
    public int currentCurve = 0;
    public float progress;
    public bool lookForward;
    public float slopeSpeedModifier;
    public float speedIncreaseCountdown;
    public float speedIncreaseRate;
    public float maximumSpeed;
    public float horizontalAdjust;
    public float verticalAdjust;
    public float moveSpeed;
    public float pathDetectionRadius;
    public float pathChangeRadius;

    public Vector3 targetPosition;
    private SplineCurve leftSpline; // Left spline found on detection
    private SplineCurve rightSpline; // Right spline found on detection
    private SplineCurve nextSpline; // Selected spline to select automatically
    private Vector3 adjustedTargetPosition;
    private SubPath[] subPaths;
    public float baseSpeed;
    private bool leader;
    private Posing posing;
    private GameManager gm;
    private Image leftPointer;
    private Image rightPointer;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        MoveTarget(1);
        subPaths = FindObjectsOfType<SubPath>();
        baseSpeed = moveSpeed;
        posing = gameObject.GetComponent<Posing>();
        foreach(Image img in GetComponentsInChildren<Image>()) {
            if (img.name == "LeftPathSprite") {
                leftPointer = img;
            }
            if (img.name == "RightPathSprite") {
                rightPointer = img;
            }
        }

        currentCurve = Mathf.FloorToInt(spline.GetPositionOnSpline(transform.position));
        progress = spline.GetPositionOnSpline(transform.position) - currentCurve;
        targetPosition = spline.GetPointOnCurve(currentCurve, progress);
    }

    private void Update()
    {
        stepSize = Mathf.Clamp(Mathf.FloorToInt(moveSpeed) * 10, 50, 200);
        if (speedIncreaseCountdown <= 0)
        {
            moveSpeed = Mathf.Clamp(moveSpeed + Time.deltaTime * speedIncreaseRate, 0, gm.globalSpeedCap);
            //baseSpeed = Mathf.Clamp(baseSpeed + Time.deltaTime * (speedIncreaseRate / 4), 0, gm.globalSpeedCap);
        }
        else
        {
            speedIncreaseCountdown -= Time.deltaTime;
        }
        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, adjustedTargetPosition) <= stepDistance)
        {
            MoveTarget(stepSize);
        }

        // Detect nearby splines
        leftSpline = null;
        rightSpline = null;
        if (subPaths.Length > 0) {
            foreach (SubPath path in subPaths) {
                if (Vector3.Distance(adjustedTargetPosition, path.transform.position) <= pathDetectionRadius &&
                    path.startingSpline == spline && !nextSpline && spline.GetPositionOnSpline(transform.position) <= path.GetPositionOnStartingSpline()) {
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

        else {
            subPaths = FindObjectsOfType<SubPath>();
        }

        if (nextSpline)
        {
            if (Vector3.Distance(targetPosition, nextSpline.transform.position) <= pathChangeRadius)
            {
                ChangeSpline(nextSpline);
                nextSpline = null;
            }
            if (Vector3.Distance(targetPosition, nextSpline.transform.position) >= pathDetectionRadius)
            {
                nextSpline = null;
            }
        }

        if (leftSpline) {
            leftPointer.enabled = true;
        }
        else {
            leftPointer.enabled = false;
        }
        if (rightSpline) {
            rightPointer.enabled = true;
        }
        else {
            rightPointer.enabled = false;
        }
    }

    // Moves the target point along the spline
    private void MoveTarget(float amount)
    {
        if (spline) {
            if (progress >= 1f) {
                if (currentCurve / 3 < spline.CurveCount - 1) {
                    currentCurve += 3;
                    progress -= 1f;
                }
                else {
                    if (spline.Loop) {
                        currentCurve = 0;
                        progress -= 1f;
                        GetComponent<PlayerStats>().laps++;
                    }
                    else {
                        if (spline.gameObject.GetComponent<SubPath>()) {
                            spline.gameObject.GetComponent<SubPath>().LeaveSpline(this);
                        }
                        else {
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
    }

    private void MoveTowardsTarget()
    {
        adjustedTargetPosition = transform.InverseTransformPoint(targetPosition);
        adjustedTargetPosition.x += horizontalAdjust;
        adjustedTargetPosition.y += verticalAdjust;
        adjustedTargetPosition = transform.TransformPoint(adjustedTargetPosition);
        Vector3 position = Vector3.MoveTowards(transform.position, adjustedTargetPosition, moveSpeed - (Vector3.Dot(transform.forward, Vector3.up) * slopeSpeedModifier));

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

    public void InitialiseRunner(SplineCurve startingSpline, float startingPosition)
    {
        currentCurve = 0;
        spline = startingSpline;
        progress = startingPosition;
        transform.localPosition = spline.GetPointOnCurve(0, progress);
        transform.LookAt(spline.GetVelocity(progress));
        targetPosition = spline.GetPointOnCurve(0, progress);
        MoveTarget(stepSize);
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void ResetSpeed() {
        moveSpeed = baseSpeed;
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

    public void TemporaryBoost(float amount, float duration)
    {
        StartCoroutine(TemporaryBoostIE(amount, duration));
    }

    private IEnumerator TemporaryBoostIE(float amount, float duration)
    {
        IncreaseSpeed(amount);
        yield return new WaitForSeconds(duration);
        IncreaseSpeed(-amount);
    }

    public void FailObstacle() {
        moveSpeed = baseSpeed;
        TemporaryBoost(-(baseSpeed / 2), 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Obstacle"))
        {
            moveSpeed = baseSpeed;
            TemporaryBoost(-(baseSpeed / 2), 1);
        }

        if (other.CompareTag("Wall"))
        {
            //Vector3 lastPosition = Vector3.zero;
            //float lastPositionFloat = 10000;
            //float lastProgress = 10000;
            //int lastCurve = 10000;
            //foreach (PlayerStats stat in FindObjectsOfType<PlayerStats>())
            //{
            //    if (stat.GetPosition() < lastPositionFloat)
            //    {
            //        lastPositionFloat = stat.GetPosition();
            //        lastPosition = stat.transform.position;
            //        lastProgress = stat.GetComponent<FollowTrack>().progress;
            //        lastCurve = stat.GetComponent<FollowTrack>().currentCurve;
            //    }
            //}
            //targetPosition = spline.GetPoint(spline.GetPositionOnSpline(lastPosition));
            //adjustedTargetPosition = spline.GetPoint(spline.GetPositionOnSpline(lastPosition));
            //currentCurve = lastCurve;
            //progress = lastProgress;
            //transform.position = targetPosition;

            //if (other.transform.gameObject.name.Contains(posing.GetPose()))
            //{
            //    PlayerPassed();                
            //}
            //else
            //{                 
            //    if (!leader && !other.transform.gameObject.name.Contains("Wall "))
            //        PunishPlayer();
            //    else BreakWall();
            //}
        }
    }

    private void BreakWall()
    {
        
    }

    private void PunishPlayer()
    {
        
    }

    private void PlayerPassed()
    {
        
    }

    public void TurnLeft() {
        if (leftSpline && leftSpline != spline) {
            nextSpline = leftSpline;
            leftSpline = null;
            rightSpline = null;
        }
    }

    public void TurnRight() {
        if (rightSpline && rightSpline != spline) {
            nextSpline = rightSpline;
            leftSpline = null;
            rightSpline = null;
        }
    }
}
