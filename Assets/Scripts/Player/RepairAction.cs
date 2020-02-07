using UnityEngine;

public class RepairAction : PlayerAction {

    private Construct _target;
	
    public RepairAction(Player player) : base(player) { }

    public override bool ReadyToUse() {
        _target = Senses.GetRepairTarget();

        if (_target == null || Player.Scrap < _target.RepairCost) {
            return false;
        }

        return Input.GetButtonDown(InputSet.RepairButton);
    }

    protected override void OnBegin() {
        Debug.Log("Using Action : REPAIR");
        
        Player.Repair(_target);
        End();
    }

    protected override void OnUpdate() {
    }
}