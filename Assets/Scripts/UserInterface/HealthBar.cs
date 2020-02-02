using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	private static readonly Color DefaultColor = new Color(0.24f, 0.78f, 0.42f);
	private static readonly Color BrokenColor = new Color(0.63f, 0.5f, 0.4f);

	public float YOffset = 100.0f;
	public Slider Bar;
	public Image BarFill;

	private ScrapBehaviour _target;
	private Camera _camera;

	public ScrapBehaviour Target => _target;

	public void Initialize(Camera camera, ScrapBehaviour scrapBehaviour) {
	
		_camera = camera;
		
		_target = scrapBehaviour;
	}
	
	private void Update() {
		if(_target == null) return;

		Vector2 screenPos = GUIBehaviour.WorldToScreen(_target.transform.position, _camera, YOffset);
		
		GetComponent<RectTransform>().anchoredPosition = screenPos;

		Bar.value = _target.CurHealth / _target.MaxHealth;

		if (_target is Construct) {
			Construct target = _target as Construct;
			BarFill.color = target.Broken ? BrokenColor : DefaultColor;
		}
		else {
			BarFill.color = BrokenColor;
		}
	}
}