using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {
    public InputActionReference m_RespawnAction;

    [HideInInspector]
    public Checkpoint CurrentCheckpoint = null;
    private Vector3 CurrentCheckpointPos = Vector3.zero;
    private List<Checkpoint> m_Checkpoints = new List<Checkpoint>();


    void Awake() {
        DontDestroyOnLoad(this);
    }

    void Upate() {
        if (m_RespawnAction.action.WasPressedThisFrame()) {
            RestartLevel("respawn key pressed.");
        }
    }

    public void RegisterCheckpoint(Checkpoint c) {
        m_Checkpoints.Add(c);
    }

    public void DeregisterCheckpoint(Checkpoint c) {
        if (m_Checkpoints.Contains(c)) {
            m_Checkpoints.Remove(c);
        }
    }

    public void ReachedCheckpoint(Checkpoint c) {
        if (c == null) {
            return;
        }
        if (CurrentCheckpoint == null || c.CheckpointNumber > CurrentCheckpoint.CheckpointNumber) {
            Debug.Log($"Reached Checkpoint #{c.CheckpointNumber} ({c.name})");
            CurrentCheckpoint = c;
            CurrentCheckpointPos = c.RespawnLocation.position;
        }
    }

    public void RestartLevel(string reason = "") {
        Debug.Log($"Restarting Level because {reason}.");
        var scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    public void SetPlayerToSpawnLocation() {
        var spawn_pos = CurrentCheckpointPos;
        var player = GameManager.Instance.PlayerObject;

        if (!player) {
            // First start, stuff is not ready, I don't care
            Debug.Log($"No player object found to respawn at {spawn_pos}");
            return;
        }

        var rbs = player.GetComponentsInChildren<Rigidbody>();
        var fish_body = GameManager.Instance.PlayerRootRigidbody;  // Player origin is fucked
        var delta_pos = spawn_pos - fish_body.position;

        foreach (var rb in rbs) {
            rb.MovePosition(rb.position + delta_pos);
        }

        Debug.Log($"Spawning player at {spawn_pos}.");
    }
}
