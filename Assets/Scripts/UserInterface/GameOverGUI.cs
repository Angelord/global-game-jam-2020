﻿using System;
using Claw;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverGUI : GUIBehaviour {

	public void OnRestartClick() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	protected override void OnInitialize() {
		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void HandlePlayerDiedEvent(PlayerDiedEvent playerDiedEvent) {
		if (playerDiedEvent.Player == Player) {
			this.gameObject.SetActive(true);
		}
	}
}
