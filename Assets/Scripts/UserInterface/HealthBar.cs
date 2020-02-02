using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	private static readonly Color BrokenColor = new Color(0.63f, 0.5f, 0.4f);

	public float YOffset = 100.0f;
	public Slider Bar;
	public Image BarFill;

	private ScrapBehaviour _target;
	private Player _player;
	private Camera _camera;

	public ScrapBehaviour Target => _target;

	public void Initialize(Camera camera, Player player, ScrapBehaviour scrapBehaviour) {
	
		_camera = camera;

		_player = player;
		
		_target = scrapBehaviour;
	}
	
	private void Update() {
		if(_target == null) return;

		Vector3 offset = new Vector3(0.0f, _target.BarYOffset, 0.0f);
		Vector2 screenPos = GUIBehaviour.WorldToScreen(_target.transform.position + offset, _camera);
		
		GetComponent<RectTransform>().anchoredPosition = screenPos;

		Bar.value = _target.CurHealth / _target.MaxHealth;

		if (_target is Construct) {
			Construct target = _target as Construct;
			if (target.Broken) {
				BarFill.color = BrokenColor;
			}
			else {
				BarFill.color = target.Faction.Color;
			}
		}
		else if(_target is Player) {
			BarFill.color = ((Player) _target).Faction.Color;
		}
		else {
			BarFill.color = BrokenColor;
		}
	}
}