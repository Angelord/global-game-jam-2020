using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scrapper/PlyerStats", order = 1)]
public class PlayerStats : ScriptableObject {
	
	public float MovementSpeed;
	public float SalvageRate; 	// How much scrap is salvaged per second
}