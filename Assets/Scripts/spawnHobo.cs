using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnHobo : MonoBehaviour {
	TileScript parentScript;

	public GameObject hoboObject;

	scoreScript repScript;

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<TileScript>();
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();

		//StartCoroutine (spawnHobos());
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void startHobos(){
		StartCoroutine (spawnHobos());
	}

	// spawn a hobo every so often
	IEnumerator spawnHobos()
	{
		yield return new WaitForSeconds(2.0f);
		while(true)
		{
			GameObject test = Instantiate(hoboObject, new Vector3 (transform.position.x, transform.position.y+0.5f, 0), Quaternion.identity) as GameObject;
			test.GetComponent<HomelessAI>().gridPosition.x = parentScript.Position.x;
			test.GetComponent<HomelessAI>().gridPosition.y = parentScript.Position.y;
			test.GetComponent<HomelessAI>().gridScript = parentScript.parentScript;
			test.name = "hobo";
			yield return new WaitForSeconds(10.0f-(0.009f*(float)repScript.reputation));

		}
	}
}
