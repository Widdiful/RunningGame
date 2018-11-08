using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleTextHue : MonoBehaviour {

    public float speed;
    private Text text;
    private float hue;

    private void Start() {
        text = GetComponent<Text>();
    }

    private void Update() {
        text.color = Color.HSVToRGB(hue, 0.9f, 0.9f);
        hue = (hue + speed) % 1;
    }
}
