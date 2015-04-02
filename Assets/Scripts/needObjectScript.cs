using UnityEngine;
using System.Collections;

public class needObjectScript : MonoBehaviour {

	public float baseTime;
	int level = 1;
	int volunteer = 0;
	public bool inUse = false;
	bool eating = false;
	float needTime = 10.0f;
	float currentTime = 0.0f;

	public int cost;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (inUse)
		{
			if (!eating)
			{
				needTime = baseTime - level*(0.5f) - volunteer*2.0f;
				eating = true;
			}
			currentTime += Time.deltaTime;
			if (currentTime>=needTime)
			{
				// need has been fulfilled
				inUse = false;
				eating = false;
				currentTime = 0.0f;
			}
		}
	}
}
