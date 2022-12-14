using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    [Range(0.01f, 4.0f)]
    private float m_ZoomSpeed;

    [SerializeField]
    [Range(0.5f, 30.0f)]
    private float m_ZoomMin;

    [SerializeField]
    [Range(0.5f, 30.0f)]
    private float m_ZoomMax;

    [SerializeField]
    private InputActionReference m_LookAction,
        m_EnableLookAction,
        m_ZoomInAction,
        m_ZoomOutAction;

    [SerializeField]
    private Transform m_PlayerTransform;

    private float m_Pitch = 0f;
    private float m_Yaw = 0f;
    private float m_Zoom = 1f;

    void Update() {

        transform.position = m_PlayerTransform.position;

        bool move_enabled = m_EnableLookAction.action.IsPressed();
        if (move_enabled) {
            ProcessMouseLook();
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }

        var zoomout = Mathf.Abs(m_ZoomOutAction.action.ReadValue<float>());
        var zoomin = Mathf.Abs(m_ZoomInAction.action.ReadValue<float>());
        if (zoomout > 1f) {
            Zoom(+m_ZoomSpeed);
        } else if (zoomin > 1f) {
            Zoom(-m_ZoomSpeed);
        }

        var rot_pitch = Quaternion.Euler(m_Pitch, 0, 0);
        var rot_yaw = Quaternion.Euler(0, m_Yaw, 0);
        transform.localRotation = rot_yaw * rot_pitch;
    }

    void ProcessMouseLook() {
        var factor = Time.deltaTime * m_MouseSensitivity;
        var lookdelta = m_LookAction.action.ReadValue<Vector2>();
        var delta_yaw = lookdelta.x * factor;
        var delta_pitch = lookdelta.y * factor;
        m_Yaw += delta_yaw;
        m_Pitch += delta_pitch;
        m_Pitch = Mathf.Clamp(m_Pitch, m_PitchMin, m_PitchMax);
    }

    void Zoom(float delta) {
        Debug.Log("Zooming" + delta);
        m_Zoom += delta;
        m_Zoom = Mathf.Clamp(m_Zoom, m_ZoomMin, m_ZoomMax);
        var vcam = GameManager.Instance.CMBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        var follow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        follow.CameraDistance = m_Zoom;
    }


}
