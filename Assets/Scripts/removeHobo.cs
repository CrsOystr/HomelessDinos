﻿using UnityEngine;
using System.Collections;

public class removeHobo : MonoBehaviour {

	scoreScript repScript;
	ISOGRID gridScript;

	// Use this for initialization
	void Start () {
	
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();
		gridScript = GameObject.Find("MainGrid").GetComponent<ISOGRID>();
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
			if (!gridScript.readyToAdvance)
			{
				if (Mathf.Max(hobo.needsLevel) <= 0)
				{
					repScript.reputation += hobo.startHoboDifficulty;
					//repScript.currency += 300;
				}
				else
				{
					//repScript.reputation -= 3*Mathf.Max(hobo.needsLevel);
					repScript.reputation -= hobo.currentHoboDifficulty;
					repScript.currency -= (50*(2+repScript.day))*Mathf.Max(hobo.needsLevel);
				}
			}

			// remove hobo
			Destroy(collision.gameObject);
		}
	}
}
