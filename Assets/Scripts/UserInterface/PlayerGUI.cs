using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlayerGUI : MonoBehaviour {

	public Player Player;
	public Text ScrapText;

	protected Camera Camera => _camera;

	private Camera _camera;
	
	private void Start() {

		_camera = GetComponent<Canvas>().worldCamera;
		
		GUIBehaviour[] behaviours = GetComponentsInChildren<GUIBehaviour>();

		foreach (GUIBehaviour guiBehaviour in behaviours) {
			guiBehaviour.Initialize(Camera, Player);
		}
	}

	private void Update() {
		ScrapText.text = Player.Scrap.ToString("0");
	}
}