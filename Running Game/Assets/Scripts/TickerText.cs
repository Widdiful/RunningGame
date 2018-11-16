using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickerText : MonoBehaviour {

    public float speed;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update () {
        rect.anchoredPosition += new Vector2(-speed, 0);
	}
}
