using UnityEngine;

[CreateAssetMenu(fileName = "Fraction", menuName = "Scrapper/Fraction", order = 1)]
public class Fraction : ScriptableObject {
	public Color Color;

	private static Fraction _neutral;
	
	public static Fraction Neutral {
		get {
			if (_neutral == null) {
				_neutral = ScriptableObject.CreateInstance<Fraction>();
			}

			return _neutral;
		}
	}
}