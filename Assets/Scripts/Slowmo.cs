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
        var timescale_new = Time.timeScale + (delta * m_SmoothSpeed * Time.deltaTime / Time.timeScale);
        timescale_new = Mathf.Clamp(timescale_new, m_SlowSpeed, 1.0f);

        if (timescale_new <= EPSILON || float.IsNaN(timescale_new) || float.IsInfinity(timescale_new)) {
            return;  // big oof
        }

        Time.timeScale = timescale_new;

        m_SlowSource.volume = Mathf.Abs(delta) > EPSILON ? 1f : 0f;

        if (Mathf.Abs(Time.timeScale - m_TargetTimeScale) <= EPSILON) {
            Time.timeScale = Mathf.Clamp01(m_TargetTimeScale);
        }

        foreach (var source in m_AudioSources) {
            float pitch;
            if (source == m_SlowSource) {
                float t = (Time.timeScale - m_SlowSpeed) / (1f - m_SlowSpeed);
                pitch = Mathf.Lerp(2 * m_SlowSpeed, 1f, t);
            } else {
                pitch = Time.timeScale;
            }
            if (pitch < 0) {
                pitch = 0;
            }
            source.pitch = pitch;
        }

        m_Slow = slow;

        // DebugText.text = "Timescale: " + Time.timeScale;
    }
}
