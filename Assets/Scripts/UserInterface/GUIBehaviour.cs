using UnityEngine;

public class GUIBehaviour : MonoBehaviour {

	private Camera _camera;
	private Player _player;

	public Camera Camera => _camera;

	public Player Player => _player;

	public void Initialize(Camera camera, Player player) {
		_camera = camera;
		_player = player;
	}

	public static Vector2 WorldToScreen(Vector2 pos, Camera camera, float yOffset = 0.0f) {
		return (Vector2)camera.WorldToScreenPoint(pos) + new Vector2(- camera.rect.x * Screen.width, yOffset);
	}
}