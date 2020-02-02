using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

	public float Duration;
	public float FloatOffset;
	private float _start;
	private float _initialY;
	private RectTransform _rectTransform;
	private Graphic _graphic;

	public void Init(Vector2 pos) {
		_graphic = GetComponent<Graphic>();
		_rectTransform = GetComponent<RectTransform>();
		_start = Time.time;
		_rectTransform.anchoredPosition = pos;
		_initialY = pos.y;
	}

	private void Update() {

		Vector2 pos = _rectTransform.anchoredPosition;
		Color col = _graphic.color;

		float progress = (Time.time - _start) / Duration;
		col.a = 1.0f - (Time.time - _start) / Duration;
		pos.y = _initialY + FloatOffset * progress;
		
		_rectTransform.anchoredPosition = pos;
		_graphic.color = col;
		
		if (progress > 1.0f) {
			Destroy(this.gameObject);
		}
	}
}