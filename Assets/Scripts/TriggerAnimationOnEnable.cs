using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TriggerAnimationOnEnable : MonoBehaviour {

    public string TriggerName;

    private void OnEnable() {
        GetComponent<Animator>().SetTrigger(TriggerName);
    }
}