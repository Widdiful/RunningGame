using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float slowestSpeed;
    public float maximumSpeedDifference;
    public float globalSpeedCap;
    public Vector3 lastPosition;
    public FollowTrack[] activePlayers;
    public bool gameStarted;
    private GameObject mainCamera;

    private void Start() {
        mainCamera = FindObjectOfType<SpectatorCamera>().gameObject;
        mainCamera.transform.Find("Canvas").gameObject.SetActive(false);
    }

    void Update() {
        activePlayers = FindObjectsOfType<FollowTrack>();


        slowestSpeed = 0;
        if (activePlayers.Length >= 2) {
            foreach (FollowTrack track in activePlayers) {
                if (slowestSpeed == 0 || track.moveSpeed < slowestSpeed) {
                    slowestSpeed = track.moveSpeed;
                }
            }
        }
        else if (activePlayers.Length == 1 && gameStarted) {
            Debug.Log(activePlayers[0].name + " wins!");
            activePlayers[0].RemoveCamera();
            activePlayers[0].GetComponent<PlayerStats>().health = 100000;
            mainCamera.transform.Find("Canvas").gameObject.SetActive(true);
            mainCamera.transform.Find("Canvas/PlayerWin").GetComponent<Text>().text = activePlayers[0].name.Replace("_", " ");
            mainCamera.GetComponent<SpectatorCamera>().target = activePlayers[0].transform;
            foreach(ParticleSystem ps in mainCamera.GetComponentsInChildren<ParticleSystem>()) {
                ps.Play();
            }
            StartCoroutine(EndGame());
            gameStarted = false;
        }
        if (slowestSpeed == 0) {
            globalSpeedCap = 10;
        }
        else {
            globalSpeedCap = slowestSpeed + maximumSpeedDifference;
        }
    }

    IEnumerator EndGame() {
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
