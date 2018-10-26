using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private GameData gameData;
    private GameObject button_panel;
	private Button button_start;
	private Button button_options;
	private Button button_credits;
	private Button button_quit;
    private bool active;
	
	// Use this for initialization
	void Start () {
        active = true;
        button_panel = GameObject.Find("Buttons_Panel");

        button_start = GameObject.Find("Button_Start").GetComponentInChildren<Button>();
		button_options = GameObject.Find("Button_Options").GetComponentInChildren<Button>();
		button_credits = GameObject.Find("Button_Credits").GetComponentInChildren<Button>();
		button_quit = GameObject.Find("Button_Quit").GetComponentInChildren<Button>();	
		
		button_start.onClick.AddListener(StartRound);
		button_options.onClick.AddListener(ShowOptions);
		button_credits.onClick.AddListener(ShowCredtis);
		button_quit.onClick.AddListener(Quit);
	}

    internal void ToggleActive()
    {
        active = !active;
        
        button_panel.gameObject.SetActive(active);

        button_start.gameObject.SetActive(active);
        button_options.gameObject.SetActive(active);
        button_credits.gameObject.SetActive(active);
        button_quit.gameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update () {
		
	}
	
	void StartRound()
	{
        button_panel.gameObject.SetActive(false);

        button_start.gameObject.SetActive(false);
        button_options.gameObject.SetActive(false);
        button_credits.gameObject.SetActive(false);
        button_quit.gameObject.SetActive(false);
        

        GameObject.Find("GameUpdates").GetComponent<DropInRound>().Playerselect = true;
	}
	
	void ShowOptions()
	{
		
	}
	
	void ShowCredtis()
	{
		
	}
	
	void Quit()
	{
        Application.Quit();
	}
}
