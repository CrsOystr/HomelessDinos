using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	// set these externally in the settings panel
	public float camSpeed;
	public int bound;
	public int cameraLimit;

	private float scrollSpeed;

	private bool dragging;
	private Plane groundPlane;
	private Vector3 mouseDownPos;



	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y+5, transform.position.z);

		Vector3 groundNormal = new Vector3(0.0f, 1.0f, 0.0f);
		Vector3 groundPoint = new Vector3(0.0f, 0.0f, 0.0f);
		groundPlane = new Plane (groundNormal, groundPoint);
	}
	
	// Update is called once per frame
	void Update () {

		scrollSpeed = camSpeed * Time.deltaTime;

		// wasd input
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");

		transform.Translate(new Vector3(xAxisValue*scrollSpeed, yAxisValue*scrollSpeed, 0.0f));
		if (this.transform.position.x > cameraLimit)
		{
			this.transform.position = new Vector3(cameraLimit,
			                                      this.transform.position.y,
			                                      this.transform.position.z);
		}
		if (this.transform.position.x < -cameraLimit)
		{
			this.transform.position = new Vector3(-cameraLimit,
			                                      this.transform.position.y,
			                                      this.transform.position.z);
		}
		if (this.transform.position.y > cameraLimit)
		{
			this.transform.position = new Vector3(this.transform.position.x,
			                                      cameraLimit,
			                                      this.transform.position.z);
		}
		if (this.transform.position.y < 0)
		{
			this.transform.position = new Vector3(this.transform.position.x,
			                                      0,
			                                      this.transform.position.z);
		}


		// edge of screen input
		/*
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
		*/

		if(Input.GetMouseButton(0))
		{
			float hitDist;
			Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (dragging)
			{
				groundPlane.Raycast(mouse, out hitDist);
				Vector3 mousePos = mouse.GetPoint(hitDist);
				//this.transform.position += mouseDownPos - mousePos;
				if (this.transform.position.x + mouseDownPos.x- mousePos.x > -cameraLimit && 
				    this.transform.position.x + mouseDownPos.x- mousePos.x < cameraLimit)
				{
					//this.transform.position += mouseDownPos - mousePos;
					this.transform.position = new Vector3(this.transform.position.x + mouseDownPos.x- mousePos.x,
					                                      this.transform.position.y,
					                                      this.transform.position.z);
				}
				if (this.transform.position.y + mouseDownPos.y- mousePos.y > 0 && 
				    this.transform.position.y + mouseDownPos.y- mousePos.y < cameraLimit)
				{
					this.transform.position = new Vector3(this.transform.position.x,
					                                      this.transform.position.y + mouseDownPos.y- mousePos.y,
					                                      this.transform.position.z);
				}
			}
			else if(Input.GetMouseButtonDown(0))
			{
				groundPlane.Raycast(mouse, out hitDist);
				mouseDownPos = mouse.GetPoint(hitDist);
				dragging = true;
			}
		}
		else
		{
			dragging = false;
		}
	}
}
