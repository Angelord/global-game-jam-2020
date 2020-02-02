using Claw;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGUI : GUIBehaviour {

	public Text Text;
	
	private void OnEnable() {
		EventManager.AddListener<GameOverEvent>(HandleGameOverEvent);
		Text.text = "";
	}
	
	private void OnDisable() {
		EventManager.RemoveListener<GameOverEvent>(HandleGameOverEvent);
	}

	private void HandleGameOverEvent(GameOverEvent gameOverEvent) {
		Text.text = "Wins : " + Player.WinCount;
	}
}