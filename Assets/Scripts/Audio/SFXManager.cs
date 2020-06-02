using System.Collections;
using System.Collections.Generic;
using Claw;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    public AK.Wwise.Event StopRepairAudioEvent;
    
    private void Start() {
        EventManager.AddListener<ConstructRepairStartedEvent>(HandleConstructRepairStartedEvent);
        EventManager.AddListener<ConstructRepairCancelledEvent>(HandleConstructRepairCancelledEvent);
        EventManager.AddListener<AttackerAttackedEvent>(HandleAttackerAttackedEvent);
    }

    private void OnDestroy()  {
        EventManager.RemoveListener<ConstructRepairStartedEvent>(HandleConstructRepairStartedEvent);
        EventManager.RemoveListener<ConstructRepairCancelledEvent>(HandleConstructRepairCancelledEvent);
        EventManager.RemoveListener<AttackerAttackedEvent>(HandleAttackerAttackedEvent);
    }

    private void HandleConstructRepairStartedEvent(ConstructRepairStartedEvent gameEvent) {
        
        Debug.Log("PLAYING REPAIR");

        Construct robot = gameEvent.Construct;
        
        robot.AudioData.PostRepair(gameObject);
    }

    private void HandleConstructRepairCancelledEvent(ConstructRepairCancelledEvent gameEvent) {

        Debug.Log("STOPPING REPAIR");
        
        StopRepairAudioEvent.Post(gameObject);
    }

    private void HandleAttackerAttackedEvent(AttackerAttackedEvent gameEvent) {
        
        Attacker robot = gameEvent.Attacker;
        
        if(robot.AudioData == null) return;
        
        robot.AudioData.PostAttack(gameObject);
    }
}
