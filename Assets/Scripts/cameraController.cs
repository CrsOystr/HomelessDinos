using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	// set these externally in the settings panel
	public float camSpeed;
	public int bound;


	private float scrollSpeed;



	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y+5, transform.position.z);

	}
	
	// Update is called once per frame
	void Update () {

		scrollSpeed = camSpeed * Time.deltaTime;

		// wasd input
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");

		transform.Translate(new Vector3(xAxisValue*scrollSpeed, yAxisValue*scrollSpeed, 0.0f));

		// edge of screen input
		if (Input.mousePosition.x < Screen.width && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > 0)
		{
			if (Input.mousePosition.x > Screen.width - bound)
			{
				transform.position = new Vector3 (transform.position.x+scrollSpeed, transform.position.y, transform.position.z);
			}
			if (Input.mousePosition.x < bound) 
			{
				transform.position = new Vector3 (transform.position.x-scrollSpeed, transform.position.y, transform.position.z);
			}
			if (Input.mousePosition.y > Screen.height - bound)
			{
				transform.position = new Vector3 (transform.position.x, transform.position.y+scrollSpeed, transform.position.z);
			}
			if (Input.mousePosition.y < bound)
			{
				transform.position = new Vector3 (transform.position.x, transform.position.y-scrollSpeed, transform.position.z);
			}
		}
	}
}
