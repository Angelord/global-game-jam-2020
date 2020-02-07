using UnityEngine;

public class RepairAction : PlayerAction {

    private Construct _target;

    private float _duration;
    
    public override float Progress { get { return _duration / (_target.RepairCost / Stats.RepairRate); } }

    public RepairAction(Player player) : base(player) { }

    public override bool IsReadyToUse() {
        
        _target = Senses.GetRepairTarget();
        
        return ActionIsValid();
    }

    protected override void OnBegin() {
        Debug.Log("Using Action : REPAIR");

        _duration = 0.0f;
    }

    protected override void OnUpdate() {
        
        if (!ActionIsValid()) {
            
            End();
            
            return;
        }

        _duration += Time.deltaTime;

        if (Progress >= 1.0f) {
            
            Player.Repair(_target);
         
            End();
        }
    }

    private bool ActionIsValid() {
        return Input.GetButton(InputSet.RepairButton) &&
               _target != null && 
               _target.Repairable &&
               Senses.IsVisible(_target) &&
               Player.Scrap >= _target.RepairCost;
    }
}