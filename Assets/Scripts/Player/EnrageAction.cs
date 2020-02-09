
using UnityEngine;

public class EnrageAction : PlayerAction {
    
    public EnrageAction(Player player) : base(player) {
    }

    public override bool IsReadyToUse() {
        if (Time.time - Player.LastEnrageTime < Stats.EnrageFrequency) {
            return false;
        }
        
        return Input.GetButtonDown(InputSet.EnrageButton);
    }

    protected override void OnBegin() {
        Player.Enrage();
        End();
    }

    protected override void OnUpdate() {
    }
}