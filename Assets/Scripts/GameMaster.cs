using System;
using Claw;
using Claw.Chrono;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	
	public enum GameState {
		RUNNING,
		PAUSED,
		OVER
	}

	public static GameState state = GameState.RUNNING;

	public static GameMaster Find() {
		return GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
	}

	private void Awake() {
		Time.timeScale = 0.0f;
		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	public void StartGame() {
		Time.timeScale = 1.0f;
	}

	private void HandlePlayerDiedEvent(PlayerDiedEvent diedEvent) {
		CustomCoroutine.WaitOneFrameThenExecute(() => {
			EventManager.TriggerEvent(new GameOverEvent());
			Time.timeScale = 0.5f;
		});
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}