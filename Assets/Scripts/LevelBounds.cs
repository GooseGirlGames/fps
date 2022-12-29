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
            if (GameManager.Instance.PlayerObject.GetComponentInChildren<PhysicsGun>().CurrentMovingPlatform) {
                return;
            }
            m_IsReloading = true;
            GameManager.Instance.Respawner.RestartLevel($"level bounds exceeded by {other.gameObject.name}");
        }
    }
}
