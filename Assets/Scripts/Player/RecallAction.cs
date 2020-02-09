using UnityEngine;

public class RecallAction : PlayerAction {
	
    public RecallAction(Player player) : base(player) {
    }

    public override bool IsReadyToUse() {
        if (Time.time - Player.LastRecallTime < Stats.RecallFrequency) {
            return false;
        }

        return Input.GetButtonDown(InputSet.RecallButton);
    }

    protected override void OnBegin() {
        Player.Recall();
        End();
    }

    protected override void OnUpdate() {
    }
}