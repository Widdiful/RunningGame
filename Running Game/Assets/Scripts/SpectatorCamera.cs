using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour {
    public float changeTime;
    public float lerpSpeed;
    private float changeTimer;
    public Transform target;
    private GameManager gm;

    private void Start() {
        gm = FindObjectOfType<GameManager>();
        if (gm.activePlayers.Length > 0) target = gm.activePlayers[Random.Range(0, gm.activePlayers.Length)].transform;
    }

    private void Update()
    {
        changeTimer -= Time.fixedDeltaTime;
        if (changeTimer <= 0)
        {
            changeTimer = changeTime;
            if (gm.activePlayers.Length > 0) target = gm.activePlayers[Random.Range(0, gm.activePlayers.Length)].transform;
        }

        if (target) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), lerpSpeed);
        }

        int playerCount = 0;
        Vector3 centrePoint = Vector3.zero;
        foreach(FollowTrack player in gm.activePlayers) {
            centrePoint += player.transform.position;
            playerCount++;
        }

        if (playerCount > 1) {
            transform.position = (centrePoint / playerCount) + new Vector3(0, 10, 0);
        }
    }
}
