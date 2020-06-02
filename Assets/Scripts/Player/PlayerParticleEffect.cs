using Claw.Chrono;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerParticleEffect : MonoBehaviour {

    public float PlaySpeed = 1.3f;
    public float BoostSpeed = 2.0f;
    public float HideSpeed = 2.0f;
    public float InitialBoostDuration = 0.2f;
    private ParticleSystem _particleSystem;
    private bool _playing;
	
    public void Initialize(Player player) {
        
        _particleSystem = GetComponent<ParticleSystem>();

        _particleSystem.startColor = player.Faction.Color;
        
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Show() {
        if (_playing) return;
        
        Invoke("EndInitialBoost", InitialBoostDuration);
        _particleSystem.playbackSpeed = BoostSpeed;
        _particleSystem.Play();
        _playing = true;
    }

    public void Stop() {
        if (!_playing) return;
        
        _playing = false;
        CancelInvoke("EndInitialBoost");
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        _particleSystem.playbackSpeed = HideSpeed;
    }

    private void EndInitialBoost() {
        _particleSystem.playbackSpeed = PlaySpeed;
    }
}