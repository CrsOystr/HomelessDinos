using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	public List<Transform> Adjacents;
	public Vector2 Position;

	public bool selected = false;

	public Transform currentObject;
	bool objectPlaced = false;

	ISOGRID parentScript;

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<ISOGRID>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// this is for detecting player clicking tile
	void OnMouseDown() {
		parentScript.selectThisTile(gameObject);
	}

	// create object
	public void buildObject(Transform newObject)
	{
		if (!objectPlaced)
		{
			currentObject = (Transform)Instantiate (newObject, new Vector3 (transform.position.x, transform.position.y-0.5f, 0), Quaternion.identity); 
			//newCell.name = string.Format("({0},{1})",x,y);
			currentObject.parent = transform;
			currentObject.renderer.sortingOrder = 10000-((int)Position.x + (int)Position.y);
			objectPlaced = true;
		}
	}
}
