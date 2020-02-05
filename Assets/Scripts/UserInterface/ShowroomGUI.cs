using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowroomGUI : MonoBehaviour {
    
    private void Start() {
        Time.timeScale = 1;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
            BackToMain();
        }
    }

    public void BackToMain() {
        SceneManager.LoadScene("MasterScene");
    }
}