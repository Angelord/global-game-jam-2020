using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scrapper/PlyerStats", order = 1)]
public class PlayerStats : ScriptableObject {
	
	public float MovementSpeed;
	public float RecallFrequency = 4.0f;
	public float RecallDuration = 1.5f;
	public float EnrageFrequency = 2.0f;
	public float EnrageDuration = 1.2f;
	public float RepairRate = 5.0f;
	public float SalvageRate = 5.0f;
}