using UnityEngine;

public class SalvageAction : PlayerAction {
	
    private Construct _target;
	
    public SalvageAction(Player player) : base(player) {
    }

    public override bool ReadyToUse() {
        _target = Senses.GetSalvageTarget();
        return Input.GetButtonDown(InputSet.SalvageButton) && _target != null;
    }

    protected override void OnBegin() {
        Debug.Log("Using Action : SALVAGE");

        Player.Salvage(_target);
        End();
    }

    protected override void OnUpdate() {
    }
}