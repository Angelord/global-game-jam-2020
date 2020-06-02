using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scrapper/AudioData", order = 1)]
public class AudioData : ScriptableObject {
	
	public AK.Wwise.Event RepairAudioEvent;
	public AK.Wwise.Event AttackAudioEvent;

	public void PostRepair(GameObject audioPlayer) {
		RepairAudioEvent.Post(audioPlayer);
	}

	public void PostAttack(GameObject audioPlayer) {
		AttackAudioEvent.Post(audioPlayer);
	}
}
