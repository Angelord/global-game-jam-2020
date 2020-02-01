using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scrapper/PlyerStats", order = 1)]
public class PlayerStats : ScriptableObject {
	
	public float MovementSpeed;
	public float RecallFrequency = 4.0f;
	public float RecallDuration = 1.5f;

    public float Health = 100.0f;
}