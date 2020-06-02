using UnityEngine;

public class SalvageAction : PlayerAction {

    private Construct _target;
    private float _duration;

    public override float Progress { get { return _duration / (_target.CurHealth / Stats.SalvageRate); } }

    public SalvageAction(Player player) : base(player) { }

    public override bool IsReadyToUse() {
        
        _target = Senses.GetSalvageTarget();

        return ActionIsValid();
    }

    protected override void OnBegin() {
        _duration = 0.0f;

        if (_target is ScrapPile) {
            Player.SalvageScrapAudioEvent.Post(Player.gameObject);
        }
        else if (_target is Creature) {
            Player.SalvageRobotAudioEvent.Post(Player.gameObject);
        }
    }

    protected override void OnUpdate() {
        
        if (!ActionIsValid()) {
            
            End();
            
            return;
        }

        _duration += Time.deltaTime;

        if (Progress >= 1.0f) {
         
            Player.Salvage(_target);
            
            End();
        }
    }

    private bool ActionIsValid() {
        return Input.GetButton(InputSet.SalvageButton) &&
               _target != null &&
               Senses.IsVisible(_target) &&
               _target.Salvageable;
    }
}