using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slowmo : MonoBehaviour {
    [SerializeField]
    private InputActionReference m_TimeSlowAction;

    private float m_SlowSpeed = 0.1f;
    private float m_TargetTimeScale = 1.0f;
    private float m_SmoothSpeed = 12f;
    public const float EPSILON = 0.005f;

    private bool m_Slow = false;

    [SerializeField]
    private AudioSource m_SlowSource;

    private List<AudioSource> m_AudioSources;

    void Start() {
        m_AudioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
    }

    void Update() {
        bool slow = m_TimeSlowAction.action.IsPressed();
        m_TargetTimeScale = slow ? m_SlowSpeed : 1f;

        var delta = m_TargetTimeScale - Time.timeScale;
        Time.timeScale += delta * m_SmoothSpeed * Time.deltaTime / Time.timeScale;

        m_SlowSource.volume = Mathf.Abs(delta) > EPSILON ? 1f : 0f;

        if (Mathf.Abs(Time.timeScale - m_TargetTimeScale) <= EPSILON) {
            Time.timeScale = m_TargetTimeScale;
        }

        foreach (var source in m_AudioSources) {
            if (source == m_SlowSource) {
                float t = (Time.timeScale - m_SlowSpeed) / (1f - m_SlowSpeed);
                source.pitch = Mathf.Lerp(2 * m_SlowSpeed, 1f, t);
            } else {
                source.pitch = Time.timeScale;
            }
        }

        m_Slow = slow;

        // DebugText.text = "Timescale: " + Time.timeScale;
    }
}
