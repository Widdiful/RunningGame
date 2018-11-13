using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public int health;
    public int laps;
    public float positionOnSpline;
    public Text comboText;
    public Image[] hearts;

    private int combo;
    private BounceUI bounce;

    private FollowTrack track;
    private float comboHue;
    private bool dead;
    private Vector3 deadRotation;

    private void Start()
    {
        track = GetComponent<FollowTrack>();
        if (comboText) bounce = comboText.GetComponent<BounceUI>();
    }

    private void Update()
    {
        //positionOnSpline = track.spline.GetPositionOnSpline(transform.position);
        if (comboText) {
            if (combo >= 100) {
                comboText.color = Color.HSVToRGB(comboHue, 0.9f, 0.9f);
                comboHue = (comboHue + 0.1f) % 1;
            }
        }
        if (dead) {
            deadRotation *= 1f + (Time.deltaTime / 4f);
            transform.Rotate(deadRotation);
        }

    }

    public void TakeHit()
    {
        health--;
        UpdateHealthUI();
        if (health <= 0 && !dead)
        {
            Kill();
        }
    }

    private void Kill() {
        GetComponent<Animator>().enabled = false;
        track.moveSpeed = 0;
        track.speedIncreaseRate = 0;
        track.lookForward = false;
        track.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.ClampMagnitude((transform.up - transform.forward) + transform.right * UnityEngine.Random.Range(-0.5f, 0.5f), 1) * 20, ForceMode.Impulse);
        deadRotation = new Vector3(UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f));
        Transform playerCamera = transform.Find("Camera");
        Destroy(playerCamera.gameObject, 3.0f);
        transform.Find("Camera").GetComponent<PointAt>().enabled = true;
        transform.Find("Camera").SetParent(null);
        dead = true;
        Destroy(gameObject, 3.0f);
    }

    public float GetPosition() {
        return laps + positionOnSpline;
    }

    private void UpdateComboUI()
    {
        if (comboText)
        {
            comboText.text = combo.ToString();
            bounce.Bounce(2);

            if (combo <= 100)
            {
                if (combo == 100)
                {
                    comboText.transform.parent.GetComponentInChildren<ParticleSystem>().Play();
                }
                comboText.color = new Color(1, 1, 1 - (combo * 0.01f), 1);
            }

            comboText.transform.parent.GetComponentInChildren<StarRotate>().speed = Mathf.Clamp(combo * 0.1f, 1, 10);
        }
    }

    private void UpdateHealthUI() {
        for (int i = 0; i < hearts.Length; i++) {
            if (i <= health - 1) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }
    }

    internal void Passed()
    {
        //TODO: Play sound or do some visuals to let the player know he passed the wall
        //Debug.Log("passed");
        if (!dead) track.IncreaseSpeed(0.1f);
        combo++;
        UpdateComboUI();
    }

    internal void Failed()
    {
        Debug.Log("failed");
        TakeHit();
        track.FailObstacle();
        combo = 0;
        UpdateComboUI();
    }
}
