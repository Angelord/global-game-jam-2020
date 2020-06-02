
using System;
using System.Collections;
using System.Collections.Generic;
using Claw.Chrono;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CommonGUI : MonoBehaviour {

	public LerpAlpha BackgroundAlpha;
	public GameObject CreditsGUI;
	public GameObject UnitsGUI;

	public void OnStartButtonClick() {
		GameMaster.Find().StartGame();

		BackgroundAlpha.IntendedAlpha = 0.0f;

		CustomCoroutine.WaitThenExecute(0.1f, () => {
			gameObject.SetActive(false);
		});
	}

	public void OnQuitButtonClick() {
		Application.Quit();
	}

	public void OnShowroomButtonClick() {
		UnitsGUI.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnCreditsButtonClick() {
		CreditsGUI.SetActive(true);
		gameObject.SetActive(false);
	}
}