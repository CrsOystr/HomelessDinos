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


	// Use this for initialization
	void Start () {
		day = 1;
		mainGrid = GameObject.Find ("MainGrid").GetComponent<ISOGRID> ();
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
		}
	}

	//function to manually add one day.
	public void addDay(){
		day++;
	}
}
