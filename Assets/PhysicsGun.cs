using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Rigidbody m_Rigidbody;
    private int m_ArmNumber;

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
        if (m_ShootAction.action.WasPressedThisFrame()) {
            Shoot();
        }
    }

    private void ProcessMouseInput() {
        var gun_pos = PositionInScreeenSpace();
        var mouse_pos = Mouse.current.position.ReadValue();
        var dist = Vector2.Distance(gun_pos, mouse_pos);
        if (m_ArmNumber == 1) {
            Debug.Log(dist);
        }
    }

    public void Shoot() {
        Debug.Log("Boom");
        var force = (-ShootDirection) * m_GunStrength;
        m_Rigidbody.AddForce(force, ForceMode.Impulse);
        m_FishBody.AddForce(0.5f * force, ForceMode.Impulse);
    }

    public Vector2 PositionInScreeenSpace() {
        var camera = Camera.current;
        var pos = camera.WorldToScreenPoint(this.transform.position);
        return new Vector2(pos.x, pos.y);
    }
}
