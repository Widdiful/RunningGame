using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorCamera : MonoBehaviour {
    public float changeTime;
    public float lerpSpeed;
    public float changeTimer;
    public Transform target;
    public Canvas tickerTextCanvas;
    public GameObject spectatorTextPrefab;

    private GameManager gm;
    private Camera cam;
    private int previousPlayerCount;

    private void Start() {
        gm = FindObjectOfType<GameManager>();
        cam = GetComponent<Camera>();
    }

    private void Awake()
    {
        if (gm) {
            if (gm.activePlayers.Length > 0) target = gm.activePlayers[Random.Range(0, gm.activePlayers.Length)].transform;
        }
        changeTimer = 0;
    }

    private void Update()
    {
        changeTimer -= Time.fixedDeltaTime;
        if (changeTimer <= 0)
        {
            changeTimer = changeTime;
            if (gm.activePlayers.Length > 0)
            {
                Transform newTarget = gm.activePlayers[Random.Range(0, gm.activePlayers.Length)].transform;
                while (target == newTarget && gm.activePlayers.Length > 1)
                {
                    newTarget = gm.activePlayers[Random.Range(0, gm.activePlayers.Length)].transform;
                }
                target = newTarget;
            }
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
        else {
            cam.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
        }

        if (gm.gameStarted) UpdateSplitscreen();

        if (Input.GetButtonDown("UIConfirm")) changeTimer = 0;
    }

    public void UpdateSplitscreen() {
        if (gm.activePlayers.Length != previousPlayerCount) {
            float x = 0.0f, y = 0.0f, width = 1.0f, height = 1.0f;

            if (gm.maxPlayers <= 2) {

            }

            else if (gm.maxPlayers == 3) {
                bool p1 = GameObject.Find("Player_1");
                bool p2 = GameObject.Find("Player_2");
                bool p3 = GameObject.Find("Player_3");

                x = 0.5f;
                y = 0.0f;
                width = 0.5f;
                height = 0.5f;

                if (!p2) {
                    x = 0.5f;
                    y = 0.0f;
                    width = 0.5f;
                    height = 1.0f;
                }
                else if (!p3) {
                    x = 0.0f;
                    y = 0.0f;
                    width = 1.0f;
                    height = 0.5f;
                }
            }

            else {
                bool p1 = GameObject.Find("Player_1");
                bool p2 = GameObject.Find("Player_2");
                bool p3 = GameObject.Find("Player_3");
                bool p4 = GameObject.Find("Player_4");

                if ((p1 && p3) ^ (p2 && p4)) {
                    width = 0.5f;
                    if (p1 && p3) {
                        x = 0.5f;
                    }
                    else {
                        x = 0.0f;
                    }
                }
                if ((p1 && p2) ^ (p3 && p4)) {
                    height = 0.5f;
                    if (p1 && p2) {
                        y = 0.0f;
                    }
                    else {
                        y = 0.5f;
                    }
                }
                if ((!p1 && !p4) ^ (!p2 && !p3)) {
                    if (!p1 && !p4) {
                        x = 0.5f;
                    }
                    if (!p2 && !p3) {
                        x = 0.0f;
                    }
                    y = 0.0f;
                    width = 0.5f;
                    height = 0.5f;
                }

            }
            cam.rect = new Rect(new Vector2(x, y), new Vector2(width, height));
        }
        previousPlayerCount = gm.activePlayers.Length;
    }

    public void NewTicker(string player)
    {
        if (spectatorTextPrefab && tickerTextCanvas)
        {
            RectTransform rect = Instantiate(spectatorTextPrefab, tickerTextCanvas.transform).GetComponent<RectTransform>();
            rect.GetComponent<Text>().text = player + " is out!";
            rect.anchoredPosition = new Vector2(150, 25);
        }
    }
}
