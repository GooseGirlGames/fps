using UnityEngine;
using UnityEngine.InputSystem;

public class Slowmo : MonoBehaviour {
    [SerializeField]
    private InputActionReference m_TimeSlowAction;

    private float m_SlowSpeed = 0.1f;
    private float m_TargetTimeScale = 1.0f;
    private float m_SmoothSpeed = 12f;
    public const float EPSILON = 0.005f;

    void Update() {
        bool slow = m_TimeSlowAction.action.IsPressed();
        m_TargetTimeScale = slow ? m_SlowSpeed : 1f;

        var delta = m_TargetTimeScale - Time.timeScale;
        Time.timeScale += delta * m_SmoothSpeed * Time.deltaTime / Time.timeScale;

        if (Mathf.Abs(Time.timeScale - m_TargetTimeScale) <= EPSILON) {
            Time.timeScale = m_TargetTimeScale;
        }

        // DebugText.text = "Timescale: " + Time.timeScale;
    }
}
