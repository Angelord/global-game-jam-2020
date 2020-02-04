using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Fraction", menuName = "Scrapper/Fraction", order = 1)]
public class Faction : ScriptableObject {
	
	public Color Color;
	
	public Material UnitMat;
	
	public int Wins = 0;
	
	private static Faction _neutral;
    public GameObject repairFx;

    public static Faction Neutral {
		get {
			if (_neutral == null) {
				_neutral = ScriptableObject.CreateInstance<Faction>();
				_neutral.UnitMat = Resources.Load<Material>("TC_Neutral");
			}

			return _neutral;
		}
	}
	
	public bool IsEnemy(Faction other) {
		return other != this;
	}

	private void OnEnable() {
		Wins = 0;
	}
}