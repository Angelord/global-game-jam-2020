using Claw.Chrono;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerParticleEffect : MonoBehaviour {

    public float PlaySpeed = 1.3f;
    public float BoostSpeed = 2.0f;
    public float HideSpeed = 2.0f;
    public float InitialBoostDuration = 0.2f;
    private ParticleSystem _particleSystem;
    private float timeRemaining = 0.0f;
	
    public void Initialize(Player player) {
        
        _particleSystem = GetComponent<ParticleSystem>();

        _particleSystem.startColor = player.Faction.Color;
        
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Show(float duration) {


        if (timeRemaining <= 0.0f) { // If not already playing
            Invoke("EndInitialBoost", InitialBoostDuration);
            _particleSystem.playbackSpeed = BoostSpeed;
            _particleSystem.Play();
        }

        timeRemaining += duration;
    }

    public void Stop() {
        timeRemaining = 0.0f;
        CancelInvoke("EndInitialBoost");
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        _particleSystem.playbackSpeed = HideSpeed;
    }

    private void EndInitialBoost() {
        _particleSystem.playbackSpeed = PlaySpeed;
    }

    private void Update() {
        if(timeRemaining <= 0.0f) return;
        
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.0f) {
            Stop();
        }
    }
}