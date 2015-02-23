using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	public List<Transform> Adjacents;
	public Vector2 Position;

	public bool selected = false;

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
}
