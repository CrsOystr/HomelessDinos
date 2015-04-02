﻿using UnityEngine;
using System.Collections;

public class removeHobo : MonoBehaviour {

	scoreScript repScript;

	// Use this for initialization
	void Start () {
	
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();
		//repScript.reputation += 10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//printf("collision with %s", collision.gameObject.name);
		System.Console.WriteLine(collision.gameObject.name);
		if (collision.gameObject.name == "hobo")
		{
			// adjust currency and reputation
			HomelessAI hobo = collision.gameObject.GetComponent<HomelessAI>();
			if (hobo.hungerLevel <= 0)
			{
				repScript.reputation += 10;
				repScript.currency += 300;
			}
			else
			{
				repScript.reputation -= 3*hobo.hungerLevel;
			}

			// remove hobo
			Destroy(collision.gameObject);
		}
	}
}
