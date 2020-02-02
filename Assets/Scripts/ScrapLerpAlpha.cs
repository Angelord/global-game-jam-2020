using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrapLerpAlpha : MonoBehaviour {

	public float Speed = 2.5f;
	public Graphic Image;
	public float IntendedAlpha;
	
	void Awake () {
		Image = GetComponent<Graphic>();
	}
	
	void Update() {
		Color current = GetColor ();
		Color c = new Color(current.r, current.g, current.b, Mathf.Lerp(current.a, IntendedAlpha, Time.deltaTime * Speed));
		SetColor (c);
	}

	public void SetAlpha(float value) {
		Color col = Image.color;
		col.a = value;
		Image.color = col;
	}

	public Color GetColor() {
		return Image.color;
	}

	public void SetColor (Color color) {
		Image.color = color;
	}
}