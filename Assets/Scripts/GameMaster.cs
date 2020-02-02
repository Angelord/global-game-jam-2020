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
	}

	public void StartGame() {
		Time.timeScale = 1.0f;
	}
}