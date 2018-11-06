using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour {
    public float changeTime;
    public float lerpSpeed;
    private float changeTimer;
    private Transform target;

    private void Update()
    {
        changeTimer -= Time.fixedDeltaTime;
        if (changeTimer <= 0)
        {
            changeTimer = changeTime;
            target = FindObjectsOfType<FollowTrack>()[Random.Range(0, FindObjectsOfType<FollowTrack>().Length)].transform;
        }

        if (target) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), lerpSpeed);
        }

        int playerCount = 0;
        Vector3 centrePoint = Vector3.zero;
        foreach(PlayerStats player in FindObjectsOfType<PlayerStats>()) {
            if (player.gameObject.name.Contains("Player")) {
                centrePoint += player.transform.position;
                playerCount++;
            }
        }

        if (playerCount > 1) {
            transform.position = (centrePoint / playerCount) + new Vector3(0, 10, 0);
        }
    }
}
