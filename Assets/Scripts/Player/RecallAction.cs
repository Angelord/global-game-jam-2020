using UnityEngine;

public class RecallAction : PlayerAction {
    
    public RecallAction(Player player) : base(player) { }

    public override bool IsReadyToUse() {
        return !Player.Enraging && !Player.Recalling && Input.GetButton(InputSet.RecallButton);
    }

    protected override void OnBegin() {
        
        Player.StartRecall();
    }

    protected override void OnUpdate() {
        
        if (Input.GetButtonUp(InputSet.RecallButton))  {
            
            Player.StopRecall();
            
            End();
        }
    }
}