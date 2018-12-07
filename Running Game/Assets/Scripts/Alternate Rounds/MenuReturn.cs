using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuReturn : MonoBehaviour
{
    private GameObject button_panel;
    private Button button_Back;
    private bool active;
    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        active = true;
        button_panel = GameObject.Find("Buttons_Panel");

        button_Back = GameObject.Find("Button_Back").GetComponentInChildren<Button>();

        button_Back.onClick.AddListener(ReturnToMenu);

        gm = FindObjectOfType<GameManager>();
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }    

}
