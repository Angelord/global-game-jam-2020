using UnityEngine;

public class PlayerGUI : MonoBehaviour {

	public Camera Camera;
	public Player Player;

	private void Start() {
		GUIBehaviour[] behaviours = GetComponentsInChildren<GUIBehaviour>();

		foreach (GUIBehaviour guiBehaviour in behaviours) {
			guiBehaviour.Initialize(Camera, Player);
		}
	}
}