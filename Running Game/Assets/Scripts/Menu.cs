using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private GameData gameData;
	private Button button_start;
	private Button button_options;
	private Button button_credits;
	private Button button_quit;	
	
	// Use this for initialization
	void Start () {		
		
		button_start = GameObject.Find("Button_Start").GetComponentInChildren<Button>();
		button_options = GameObject.Find("Button_Options").GetComponentInChildren<Button>();
		button_credits = GameObject.Find("Button_Credits").GetComponentInChildren<Button>();
		button_quit = GameObject.Find("Button_Quit").GetComponentInChildren<Button>();	
		
		button_start.onClick.AddListener(StartRound);
		button_options.onClick.AddListener(ShowOptions);
		button_credits.onClick.AddListener(ShowCredtis);
		button_quit.onClick.AddListener(Quit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void StartRound()
	{
		Debug.Log("hihihi");
		GameObject.Find("GameUpdates").GetComponent<DropInRound>().playerselect = true;
	}
	
	void ShowOptions()
	{
		
	}
	
	void ShowCredtis()
	{
		
	}
	
	void Quit()
	{
		
	}
}
