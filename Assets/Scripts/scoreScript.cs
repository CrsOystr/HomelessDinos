using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreScript : MonoBehaviour {

	public int currency;
	public int reputation;
	public int day;

	public Text curText;
	public Text repText;
	public Text dayText;

	public int foodStorage;

	// Use this for initialization
	void Start () {
		day = 1;
	}
	
	// Update is called once per frame
	void Update () {
		curText.text = currency.ToString();
		repText.text = reputation.ToString();
		dayText.text = day.ToString();
	}

	//function to manually add one day.
	public void addDay(){
		day++;
	}
}
