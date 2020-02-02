using UnityEngine;

public enum CreatureType {
	Gunner,
	Hentairoi,
	Pistario,
	Rakabat,
	Shlurker,
	Zaratiusha
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Scrapper/AudioData", order = 1)]
public class AudioData : ScriptableObject {
	public CreatureType Race;
}
