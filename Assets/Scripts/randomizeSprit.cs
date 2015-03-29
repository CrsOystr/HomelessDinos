using UnityEngine;
using System.Collections;

public class randomizeSprit : MonoBehaviour {

	public Sprite[] randomSprites;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = randomSprites[Random.Range(0,randomSprites.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		//GetComponent<SpriteRenderer>().sprite = randomSprites[Random.Range(0,randomSprites.Length-1)];
	}
}
