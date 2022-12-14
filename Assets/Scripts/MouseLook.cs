using UnityEngine;
using UnityEngine.InputSystem;


public class MouseLook : MonoBehaviour {
    [SerializeField]
    [Range(0.5f, 30.0f)]
    private float m_MouseSensitivity;

    [SerializeField]
    [Range(0.0f, 360.0f)]
    private float m_PitchMin;

    [SerializeField]
    [Range(0.0f, 360.0f)]
    private float m_PitchMax;

    [SerializeField]
    private InputActionReference m_LookAction;

    [SerializeField]
    private Transform m_PlayerTransform;

    private float m_Pitch = 0f;
    private float m_Yaw = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

        transform.position = m_PlayerTransform.position;

        var factor = Time.deltaTime * m_MouseSensitivity;
        var lookdelta = m_LookAction.action.ReadValue<Vector2>();
        var delta_yaw = lookdelta.x * factor;
        var delta_pitch = lookdelta.y * factor;
        m_Yaw += delta_yaw;
        m_Pitch += delta_pitch;
        m_Pitch = Mathf.Clamp(m_Pitch, m_PitchMin, m_PitchMax);

        var rot_pitch = Quaternion.Euler(m_Pitch, 0, 0);
        var rot_yaw = Quaternion.Euler(0, m_Yaw, 0);
        transform.localRotation = rot_yaw * rot_pitch;
    }

}
