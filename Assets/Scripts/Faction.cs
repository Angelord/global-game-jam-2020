using UnityEngine;

[System.Serializable]
public class InputSet {
	
	public string HorizontalAxis;
	
	public string VerticalAxis;
	
	public string SalvageButton;
	
	public string RepairButton;
	
	public string UseButton;
	
	public string RecallButton;

	public string EnrageButton;
	
	public void SetIndex(int index) {
		HorizontalAxis = "Horizontal_" + index;
		VerticalAxis = "Vertical_" + index;
		SalvageButton = "Salvage_" + index;
		RepairButton = "Repair_" + index;
		UseButton = "Use_" + index;
		RecallButton = "Recall_" + index;
		EnrageButton = "Enrage_" + index;
	}
}

[CreateAssetMenu(fileName = "Fraction", menuName = "Scrapper/Fraction", order = 1)]
public class Faction : ScriptableObject {
	
	public Color Color;
	
	public Material UnitMat;
	
	public int Wins = 0;
	
    public GameObject RepairEffect;

    public int Index;
    
    [ReadOnlyField] public InputSet InputSet;
    
	private static Faction _neutral;

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

	private void OnValidate() {
		InputSet.SetIndex(Index);
	}

	private void OnEnable() {
		Wins = 0;
	}
}