using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ActionProgressGUI : GUIBehaviour {
    
    public float YOffset;
    private Slider _bar;

    protected override void OnInitialize() {
        _bar = GetComponent<Slider>();
    }

    private void Update() {
        if (Player.isDead) {
            gameObject.SetActive(false);
            return;
        }

        Vector3 offset = new Vector3(0.0f, YOffset, 0.0f);
        
        Vector2 screenPos = WorldToScreen(Player.transform.position + offset, Camera);
		
        GetComponent<RectTransform>().anchoredPosition = screenPos;
        
        _bar.value = Player.CurrentAction.IsDone ? 0.0f : Player.CurrentAction.Progress;
    }
}