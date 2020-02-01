using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

	public Camera Camera;
	public Player Player;
	public Text ScrapText;
	
	private void Start() {
		GUIBehaviour[] behaviours = GetComponentsInChildren<GUIBehaviour>();

		foreach (GUIBehaviour guiBehaviour in behaviours) {
			guiBehaviour.Initialize(Camera, Player);
		}
	}

	private void Update() {
		ScrapText.text = Player.Scrap.ToString();
	}
}