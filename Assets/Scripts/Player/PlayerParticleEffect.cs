using Claw.Chrono;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerParticleEffect : MonoBehaviour {

    private ParticleSystem _particleSystem;
    private float timeRemaining = 0.0f;
	
    public void Initialize(Player player) {
        
        _particleSystem = GetComponent<ParticleSystem>();

        _particleSystem.startColor = player.Faction.Color;
        
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Show(float duration) {
     
        timeRemaining += duration;

        _particleSystem.playbackSpeed = 1.3f;
        _particleSystem.Play();
    }

    private void Update() {
        if(timeRemaining <= 0.0f) return;
        
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.0f) {
            timeRemaining = 0.0f;

            _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            _particleSystem.playbackSpeed = 1.8f;
        }
    }
}