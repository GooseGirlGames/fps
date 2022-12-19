using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public int CheckpointNumber = -1;
    [SerializeField]
    private Transform m_RespawnLocation;

    public Transform RespawnLocation {
        get => m_RespawnLocation;
    }

    private bool m_Registered = false;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager.Instance.Respawner.ReachedCheckpoint(this);
        }
    }

    void OnEnable() {
        m_Registered = false;
    }

    void Update() {
        if (!m_Registered && GameManager.Instance.Respawner != null) {
            GameManager.Instance.Respawner.RegisterCheckpoint(this);
            m_Registered = true;
        }
    }

    void OnDisable() {
        GameManager.Instance.Respawner.DeregisterCheckpoint(this);
    }
}
