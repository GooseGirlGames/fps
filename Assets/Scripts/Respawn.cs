using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {
    public InputActionReference m_RespawnAction;

    void Awake() {
        // DontDestroyOnLoad(this);
    }

    void Upate() {
        if (m_RespawnAction.action.WasPressedThisFrame()) {
            RestartLevel();
        }
    }

    public void RestartLevel() {
        var scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }
}
