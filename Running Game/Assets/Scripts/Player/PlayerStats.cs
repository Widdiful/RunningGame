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
    private int combo;
    private BounceUI bounce;

    private FollowTrack track;
    private float comboHue;

    private void Start()
    {
        track = GetComponent<FollowTrack>();
        if (comboText) bounce = comboText.GetComponent<BounceUI>();
    }

    private void Update()
    {
        //positionOnSpline = track.spline.GetPositionOnSpline(transform.position);
        if (combo >= 100)
        {
            comboText.color = Color.HSVToRGB(comboHue, 0.9f, 0.9f);
            comboHue = (comboHue + 0.1f) % 1;
        }
    }

    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
        }
    }

    internal void Passed()
    {
        //TODO: Play sound or do some visuals to let the player know he passed the wall
        //Debug.Log("passed");
        track.IncreaseSpeed(0.1f);
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
