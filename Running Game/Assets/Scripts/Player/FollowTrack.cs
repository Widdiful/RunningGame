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
    public float positionOnSpline = 0;
    public bool randomStart;

    public Vector3 targetPosition;
    private SplineCurve leftSpline; // Left spline found on detection
    private SplineCurve rightSpline; // Right spline found on detection
    private SplineCurve nextSpline; // Selected spline to select automatically
    private Vector3 adjustedTargetPosition;
    private SubPath[] subPaths;
    private Wall[] walls;
    public Vector3 nextWallPos;
    public float baseSpeed;
    private bool leader;
    private Posing posing;
    private GameManager gm;
    private Image leftPointer;
    private Image rightPointer;
    public Transform targetPositionObj;
    public Transform adjustedTargetPositionObj;
    public float distanceToNextWall;
    public Text speedText;
    private float speedometerHue;
    private float deFactoSpeed;
    private Vector3 previousPosition;
    private bool canConfetti = true;
    private ParticleSystem speedLines;


    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        MoveTarget(1);
        subPaths = FindObjectsOfType<SubPath>();
        walls = FindObjectsOfType<Wall>();
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
        previousPosition = transform.position;
        if (GameObject.Find(transform.name + "/Canvas/SpeedLines"))
            speedLines = GameObject.Find(transform.name + "/Canvas/SpeedLines").GetComponent<ParticleSystem>();

        //nextWallPos = GetNextWallPosition();
        if (randomStart)
        {
            transform.position = spline.GetPoint(UnityEngine.Random.Range(0.0f, 1.0f));
            float newProgress = spline.GetNearestPointFromVector(transform.position);
            currentCurve = Mathf.FloorToInt(newProgress) * 3;
            progress = newProgress - Mathf.FloorToInt(newProgress);
            targetPosition = spline.GetPointOnCurve(currentCurve, progress);
        }
    }

    private void Update()
    {
        deFactoSpeed = Vector3.Distance(transform.position, previousPosition);
        previousPosition = transform.position;
        positionOnSpline = spline.GetPositionOnSpline(transform.position);
        stepSize = Mathf.Clamp(Mathf.FloorToInt(moveSpeed) * 10, 50, 200);
        if (speedIncreaseCountdown <= 0)
        {
            moveSpeed = Mathf.Clamp(moveSpeed + Time.deltaTime * speedIncreaseRate, baseSpeed, gm.globalSpeedCap);
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

        // Detect next wall
        nextWallPos = GetNextWallPosition();
        distanceToNextWall = Vector3.Distance(nextWallPos, transform.position);

        if (distanceToNextWall <= 80 && !posing.posePromptActive)
        {
            posing.RandomPosePrompt();
        }
        else if (distanceToNextWall > 80 && posing.posePromptActive)
        {
            posing.posePromptActive = false;
            posing.posePromptPossible = true;
        }

        // Detect nearby splines
        leftSpline = null;
        rightSpline = null;
        if (subPaths.Length > 0) {
            foreach (SubPath path in subPaths) {
                if (Vector3.Distance(adjustedTargetPosition, path.transform.position) <= pathDetectionRadius &&
                    path.startingSpline == spline && !nextSpline && positionOnSpline <= path.GetPositionOnStartingSpline()) {
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
            if (leftPointer) leftPointer.enabled = true;
        }
        else {
            if (leftPointer) leftPointer.enabled = false;
        }
        if (rightSpline) {
            if (rightPointer) rightPointer.enabled = true;
        }
        else {
            if (rightPointer) rightPointer.enabled = false;
        }

        if (speedLines)
        {
            ParticleSystem.MainModule psMain = speedLines.main;
            psMain.startColor = new Color(1, 1, 1, Mathf.Clamp01((moveSpeed - 5) / 5f));
            psMain.startSpeed = ((moveSpeed - 5) / 5f) * 50;
            ParticleSystem.EmissionModule emission = speedLines.emission;
            emission.rateOverTime = ((moveSpeed - 5) / 5f) * 500;
        }

        UpdateSpeedUI();
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
        targetPositionObj.position = targetPosition;
        targetPositionObj.LookAt(spline.GetPoint(spline.GetNearestPointFromVector(targetPosition) + 0.01f));
        targetPositionObj.LookAt(targetPosition + spline.GetVelocityOnCurve(currentCurve, progress).normalized);
        adjustedTargetPositionObj.localPosition = new Vector3(horizontalAdjust, verticalAdjust, 0);
        adjustedTargetPosition = adjustedTargetPositionObj.position;
        //adjustedTargetPosition = transform.InverseTransformPoint(targetPosition);
        //adjustedTargetPosition.x += horizontalAdjust;
        //adjustedTargetPosition.y += verticalAdjust;
        //adjustedTargetPosition = transform.TransformPoint(adjustedTargetPosition);

        float adjustedMoveSpeed = moveSpeed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (hit.transform.GetComponent<FollowTrack>())
            {
                adjustedMoveSpeed = Mathf.Clamp(moveSpeed, 0, hit.transform.GetComponent<FollowTrack>().moveSpeed);
            }
        }

        Vector3 position = Vector3.MoveTowards(transform.position, adjustedTargetPosition, adjustedMoveSpeed - (Vector3.Dot(transform.forward, Vector3.up) * slopeSpeedModifier));

        if (lookForward)
        {
            if (adjustedTargetPosition - transform.position != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(adjustedTargetPosition - transform.position), 0.1f);
            }
        }
        transform.localPosition = position;
        //Debug.DrawLine(transform.position, adjustedTargetPosition);
        //Debug.DrawLine(adjustedTargetPosition, targetPosition);
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
            //nextWallPos = GetNextWallPosition();

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

    public void CenterPlayer()
    {
        horizontalAdjust = 0;
    }

    public void TurnLeft() {
        if (leftSpline && leftSpline != spline) {
            nextSpline = leftSpline;
            leftSpline = null;
            rightSpline = null;
        }
        else if (distanceToNextWall >= 20)
        {
            horizontalAdjust = -2;
        }
        else
        {
            horizontalAdjust = 0;
        }
    }

    public void TurnRight() {
        if (rightSpline && rightSpline != spline) {
            nextSpline = rightSpline;
            leftSpline = null;
            rightSpline = null;
        }
        else if (distanceToNextWall >= 20)
        {
            horizontalAdjust = 2;
        }
        else
        {
            horizontalAdjust = 0;
        }
    }

    private Vector3 GetNextWallPosition()
    {
        float tempClosest = 2;
        Vector3 wallPos = new Vector3();
        foreach (Wall wall in walls)
        {
            if (wall.positionOnSpline < tempClosest && wall.positionOnSpline > positionOnSpline)
            {
                tempClosest = wall.positionOnSpline;
                wallPos = wall.transform.position;
            }
        }
        foreach (Wall wall in walls)
        {
            if (wall.positionOnSpline + 1 < tempClosest && wall.positionOnSpline + 1 > positionOnSpline)
            {
                tempClosest = wall.positionOnSpline + 1;
                wallPos = wall.transform.position;
            }
        }
        return wallPos;
    }

    private void UpdateSpeedUI()
    {
        if (speedText)
        {
            speedText.text = Mathf.FloorToInt(moveSpeed * 100).ToString();
            if (moveSpeed >= 10)
            {
                if (canConfetti)
                {
                    speedText.transform.parent.GetComponentInChildren<ParticleSystem>().Play();
                    canConfetti = false;
                }
                speedText.color = Color.HSVToRGB(speedometerHue, 0.9f, 0.9f);
                speedometerHue = (speedometerHue + 0.1f) % 1;
            }
            else
            {
                speedText.color = new Color(1, 1, 1 - (moveSpeed * 0.1f), 1);
                canConfetti = true;
            }
        }
    }

    public void RemoveCamera() {
        Camera[] cameras = GetComponentsInChildren<Camera>();
        foreach (Camera cam in cameras) {
            StartCoroutine(MoveCamera(cam));
        }
    }

    IEnumerator MoveCamera(Camera camera) {
        bool loop = true;
        float moveAmount = 0.01f;
        if (camera.rect.x >= 0.5f) {
            moveAmount *= -1;
        }
        while (loop) {
            camera.rect = new Rect(new Vector2(camera.rect.x - moveAmount, camera.rect.y), new Vector2(camera.rect.width, camera.rect.height));
            if (Mathf.Abs(camera.rect.x) >= 1) {
                loop = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
