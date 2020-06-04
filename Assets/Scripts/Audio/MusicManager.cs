using System;
using Claw;
using UnityEngine;

namespace Audio {
    public class MusicManager : MonoBehaviour {

        private static bool musicInitialized = false;

        public Player Player1;
        public Player Player2;
        public float BattleMusicDistance;
        public float BattleMusicStopDuration;
        
        public AK.Wwise.Event MusicStartEvent;
        public AK.Wwise.State PlayMusicState;
        public AK.Wwise.State PauseMusicState;
        public AK.Wwise.State BattleMusicState;
        public AK.Wwise.State ExploreMusicState;
        private float timeOfLastContact;
        private bool playingBattleMusic = false;

        private void Start() {

            if (!musicInitialized) {
                MusicStartEvent.Post(gameObject);
                musicInitialized = true;
            }

            EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
            EventManager.AddListener<GameStartedEvent>(HandleGameStartedEvent);
        }

        private void OnDestroy() {
            EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
            EventManager.RemoveListener<GameStartedEvent>(HandleGameStartedEvent);
        }

        private void Update() {

            float distance = Vector2.Distance(Player1.transform.position, Player2.transform.position);

            bool insideRange = distance <= BattleMusicDistance;
            
            if (insideRange) { 
                HandleInRange();
            }
            else {
                HandleOutsideRange();
            }
        }

        private void HandleInRange() {
            timeOfLastContact = Time.time; 
            
            if(playingBattleMusic) return;

            Debug.Log("Audio : Playing battle music");
            BattleMusicState.SetValue();
            playingBattleMusic = true;
        }

        private void HandleOutsideRange() {
            if (!playingBattleMusic) return;
            
            float timeSinceLastInRange = Time.time - timeOfLastContact;

            if (timeSinceLastInRange >= BattleMusicStopDuration) {
                Debug.Log("Audio : Playing explore music");
                ExploreMusicState.SetValue();
                playingBattleMusic = false;
            }
        }

        private void HandleGameStartedEvent(GameStartedEvent gameStartedEvent) {
            PlayMusicState.SetValue();
        }

        private void HandlePlayerDiedEvent(PlayerDiedEvent gameOverEvent) {
            PauseMusicState.SetValue();
        }
    }
}
