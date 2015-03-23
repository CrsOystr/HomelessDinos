using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnHobo : MonoBehaviour {
	TileScript parentScript;

	public GameObject hoboObject;

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<TileScript>();

		StartCoroutine (spawnHobos());
	}
	
	// Update is called once per frame
	void Update () {

	}

	// spawn a hobo every so often
	IEnumerator spawnHobos()
	{
		yield return new WaitForSeconds(2.0f);
		while(true)
		{
			GameObject test = Instantiate(hoboObject, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
			test.GetComponent<HomelessAI>().gridPosition.x = parentScript.Position.x;
			test.GetComponent<HomelessAI>().gridPosition.y = parentScript.Position.y;
			test.GetComponent<HomelessAI>().gridScript = parentScript.parentScript;
			yield return new WaitForSeconds(5.0f);

		}
	}
}
