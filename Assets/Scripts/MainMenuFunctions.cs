﻿using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ContinueGame()
	{
		PlayerPrefs.SetInt("loadSave",1);
		PlayerPrefs.Save();
		ChangeScene(1);
	}

	public void newGame()
	{
		PlayerPrefs.SetInt("loadSave",0);
		PlayerPrefs.Save();
		ChangeScene(1);
	}

	public void ChangeScene(int newScene)
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel(newScene);
	}

	public void CloseGame()
	{
		Application.Quit();
	}

	public void pauseGame(bool pause)
	{
		if (pause)
		{
			Time.timeScale = 0.0f;
		}
		else
		{
			Time.timeScale = 1.0f;
		}
	}
}
