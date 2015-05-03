using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreScript : MonoBehaviour {

	public ISOGRID mainGrid;

	public int currency;
	public int reputation;
	public int day;

	public Text curText;
	public Text repText;
	public Text dayText;
	public Text tileText;


	public int foodStorage;

	public GameObject endScreen;
	public GameObject blockingPanel;

	public MainMenuFunctions menuFunctions;

	public GameObject introScreen;
	public GameObject continueScreen;
	public GameObject[] panels;


	// Use this for initialization
	void Start () {
		day = 1;
		mainGrid = GameObject.Find ("MainGrid").GetComponent<ISOGRID> ();

		if (PlayerPrefs.GetInt("loadSave") == 1)
		{
			currency = PlayerPrefs.GetInt("currency");
			reputation = PlayerPrefs.GetInt("reputation");
			day = PlayerPrefs.GetInt("day");
			continueScreen.SetActive(true);
			foreach(GameObject obj in panels)
			{
				obj.SetActive(true);
			}
		}
		else
		{
			PlayerPrefs.SetInt("currency", currency);
			PlayerPrefs.SetInt("reputation", reputation);
			PlayerPrefs.SetInt("day", day);
			introScreen.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		curText.text = currency.ToString();
		repText.text = reputation.ToString();
		dayText.text = day.ToString();
		tileText.text = mainGrid.numPaths.ToString ();


		if (currency < 0)
		{
			endScreen.SetActive(true);
			blockingPanel.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	//function to manually add one day.
	public void addDay(){
		day++;
	}

	public void SaveAndQuit()
	{
		mainGrid.clearGrid();

		PlayerPrefs.SetInt("currency", currency);
		PlayerPrefs.SetInt("reputation", reputation);
		PlayerPrefs.SetInt("day", day);


		menuFunctions.ChangeScene(0);

	}
}
