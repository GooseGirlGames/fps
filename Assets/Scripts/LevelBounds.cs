using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBounds : MonoBehaviour {

    private bool m_IsReloading = false;

    void Start() {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    public void OnSceneChanged(Scene scene, LoadSceneMode mode) {
        m_IsReloading = false;
    }

    public void OnTriggerExit(Collider other) {
        if (!m_IsReloading && (other.gameObject.CompareTag("Player"))) {
            m_IsReloading = true;
            GameManager.Instance.Respawn.RestartLevel();
        }
    }
}
