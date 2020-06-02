using UnityEngine;

public class EnrageAction : PlayerAction {
    
    public EnrageAction(Player player) : base(player) {
    }

    public override bool IsReadyToUse() {
        return !Player.Recalling && !Player.Enraging && Input.GetButton(InputSet.EnrageButton);
    }

    protected override void OnBegin() {
     
        Player.StartEnrage();
    }

    protected override void OnUpdate() {
        
        if (Input.GetButtonUp(InputSet.EnrageButton)) {
            
            Player.StopEnrage();
            
            End();
        }
    }
}