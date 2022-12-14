using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PhysicsGun : MonoBehaviour {

    [SerializeField]
    private InputActionReference m_ShootAction;

    [SerializeField]
    private Transform m_GunSource;

    [SerializeField]
    [Range(0.01f, 60f)]
    private float m_GunStrength;

    [SerializeField]
    private Rigidbody m_FishBody;

    [SerializeField]
    private LineRenderer m_LineRend;

    [SerializeField]
    private TMP_Text m_DebugText;

    private Rigidbody m_Rigidbody;
    private int m_ArmNumber;
    private const float DISTANCE_THRES = 0.06f;
    private bool m_InRange = false;

    public Vector3 ShootDirection {
        get => m_GunSource.forward;
    }

    public string KeyName {
        get => m_ShootAction.action.bindings[0].ToDisplayString();
    }

    private bool m_LineRendEnabled = false;

    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        var actionname = m_ShootAction.action.name;
        var actionid_str = actionname[actionname.Length - 1];
        m_ArmNumber = Int32.Parse("" + actionid_str);
    }

    void Update() {
        ProcessInputs();
        ProcessMouseInput();

        m_LineRend.enabled = m_LineRendEnabled;
        if (m_LineRendEnabled) {
            var line_length = 20f;
            UpdateLineRendererPositions(transform.position, transform.position + (line_length * ShootDirection));
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_GunSource.position, m_GunSource.position + 5f * ShootDirection);
    }

    private void UpdateLineRendererPositions(Vector3 origin, Vector3 destination) {
        m_LineRend.SetPosition(0, origin);
        m_LineRend.SetPosition(1, destination);
    }

    private void ProcessInputs() {
        if (m_ShootAction.action.WasPressedThisFrame() && m_InRange) {
            Shoot();
        }
    }

    private void ProcessMouseInput() {
        var gun_pos = PositionInScreeenSpace();
        var mouse_pos = Mouse.current.position.ReadValue();

        var screen_dimensions = new Vector3(Screen.width, Screen.height);
        gun_pos = Util.DivideElementWise2(gun_pos, screen_dimensions);
        mouse_pos = Util.DivideElementWise2(mouse_pos, screen_dimensions);

        var dist = Vector2.Distance(gun_pos, mouse_pos);
        if (m_ArmNumber == 1) {
            var cam = Camera.main;
            Vector3 target = cam.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, 1));
            Debug.DrawLine(m_GunSource.position, target);
            if (m_DebugText) {
                m_DebugText.text = "Dist: " + Mathf.Round(dist * 100f) / 100f;
            }
        }

        m_InRange = dist <= DISTANCE_THRES;
        m_LineRendEnabled = m_InRange;
    }

    public void Shoot() {
        Debug.Log("Boom");
        var force = (-ShootDirection) * m_GunStrength;
        m_Rigidbody.AddForce(force, ForceMode.Impulse);
        m_FishBody.AddForce(0.5f * force, ForceMode.Impulse);
    }

    public Vector2 PositionInScreeenSpace() {
        var camera = Camera.main;
        if (!camera || !m_GunSource) {
            if (!camera) {
                Debug.LogWarning("No camera found in Tentacle " + m_ArmNumber);
            }
            if (!m_GunSource) {
                Debug.LogWarning("No gun source for Tentacle " + m_ArmNumber);
            }
            return Vector2.zero;
        }
        var pos = camera.WorldToScreenPoint(m_GunSource.position);
        return new Vector2(pos.x, pos.y);
    }
}
