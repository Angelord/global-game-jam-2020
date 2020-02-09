using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowroomGUI : MonoBehaviour {

    public GameObject CommonGUI;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
            BackToMain();
        }
    }

    public void BackToMain() {
        CommonGUI.SetActive(true);
        gameObject.SetActive(false);
    }
}