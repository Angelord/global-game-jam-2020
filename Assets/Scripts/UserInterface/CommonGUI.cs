
using System;
using System.Collections;
using System.Collections.Generic;
using Claw.Chrono;
using UnityEngine;
using UnityEngine.UI;


public class CommonGUI : MonoBehaviour {

	public LerpAlpha BackgroundAlpha;
	
	public void OnStartButtonClick() {
		GameMaster.Find().StartGame();
		
		BackgroundAlpha.IntendedAlpha = 0.0f;
		
		CustomCoroutine.WaitThenExecute(0.1f, () => {
			this.gameObject.SetActive(false);
		});
	}

	public void OnQuitButtonClick() {
		Application.Quit();
	}
}