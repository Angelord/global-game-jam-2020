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
}