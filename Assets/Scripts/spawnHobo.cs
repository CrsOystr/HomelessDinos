using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnHobo : MonoBehaviour {
	TileScript parentScript;

	public GameObject hobo1;
	public GameObject hobo2;
	public GameObject hobo3;
	public GameObject hobo4;
	public GameObject hobo5;
	public GameObject[] go;

	bool running = true;

	scoreScript repScript;

	// Use this for initialization
	void Start () {
		go = new GameObject[5];
		go [0] = hobo1;
		go [1] = hobo2;
		go [2] = hobo3;
		go [3] = hobo4;
		go [4] = hobo5;


		parentScript = transform.parent.GetComponent<TileScript>();
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();

		//StartCoroutine (spawnHobos());
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void startHobos(){
		running = true;
		StartCoroutine (spawnHobos());
	}

	public void stopHobos()
	{
		running = false;
	}

	// spawn a hobo every so often
	IEnumerator spawnHobos()
	{

		yield return new WaitForSeconds(2.0f);
		while(true)
		{
			if (running == false)
			{
				yield break;
			}
			GameObject test = Instantiate(go[Random.Range(0,5)], new Vector3 (transform.position.x, transform.position.y+0.5f, 0), Quaternion.identity) as GameObject;
			test.GetComponent<HomelessAI>().gridPosition.x = parentScript.Position.x;
			test.GetComponent<HomelessAI>().gridPosition.y = parentScript.Position.y;
			test.GetComponent<HomelessAI>().gridScript = parentScript.parentScript;
			test.GetComponent<HomelessAI>().difficulty = repScript.reputation;
			test.GetComponent<Animator>();
			test.name = "hobo";
			// how fast hobos spawn
			yield return new WaitForSeconds(10.0f-(0.009f*(float)repScript.reputation));


		}
	}
}
