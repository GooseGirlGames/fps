using UnityEngine;
using UnityEngine.InputSystem;

public class Slowmo : MonoBehaviour {
    [SerializeField]
    private InputActionReference m_TimeSlowAction;

    private float m_SlowSpeed = 0.1f;

    void Update() {
        bool slow = m_TimeSlowAction.action.IsPressed();
        Time.timeScale = slow ? m_SlowSpeed : 1f;
    }
}
