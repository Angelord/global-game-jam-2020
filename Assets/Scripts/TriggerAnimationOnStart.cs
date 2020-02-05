using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TriggerAnimationOnStart : MonoBehaviour {

    public string TriggerName;

    private void Start() {
        GetComponent<Animator>().SetTrigger(TriggerName);
    }
}