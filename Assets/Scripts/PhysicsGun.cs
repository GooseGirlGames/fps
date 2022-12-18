using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PhysicsGun : MonoBehaviour {

    [SerializeField]
    private SkinnedMeshRenderer m_ArmRenderer;

    [SerializeField]
    private InputActionReference m_ShootAction;

    [SerializeField]
    private InputActionReference m_MachineGunAction;

    [SerializeField]
    private MuzzleFlash m_MuzzleFlash;

    [SerializeField]
    private Transform m_GunSource;

    private float m_GunStrength = 60f;
    private float m_MinUpwardsForce = 2f;

    [SerializeField]
    private Rigidbody m_FishBody;
    private List<Rigidbody> m_FishBodyRigidbodies;

    [SerializeField]
    private LineRenderer m_LineRend;

    [SerializeField]
    private TMP_Text m_DebugText;

    [SerializeField]
    private AudioSource m_GunshotAudio;

    [SerializeField]
    private Material m_HighlightMaterial;
    [SerializeField]
    private Material m_HighlightMaterialSecondary;
    private Material m_RegularMaterial;
    private Material m_RegularMaterialSecondary;


    private Rigidbody m_Rigidbody;
    private int m_ArmNumber;
    private static int ArmCount = 0;
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
        // var actionname = m_ShootAction.action.name;
        // var actionid_str = actionname[actionname.Length - 1];
        // m_ArmNumber = Int32.Parse("" + actionid_str);
        m_ArmNumber = ++ArmCount;
        m_RegularMaterial = m_ArmRenderer.materials[0];
        m_RegularMaterialSecondary = m_ArmRenderer.materials[1];

        var rbs = m_FishBody.transform.parent.GetComponentsInChildren<Rigidbody>();
        m_FishBodyRigidbodies = new List<Rigidbody>(rbs);

        // Setup GameManger's PlayerObject
        if (GameManager.Instance.PlayerObject == null) {
            var go = this.gameObject;
            while (go.transform.parent != null) {
                go = go.transform.parent.gameObject;
            }
            GameManager.Instance.PlayerObject = go;
        }
        if (GameManager.Instance.PlayerRootRigidbody == null && m_FishBody != null) {
            GameManager.Instance.PlayerRootRigidbody = m_FishBody;
        }
    }

    void FixedUpdate() {
        if (m_MachineGunAction.action.IsPressed()) {
            float t = Time.time * 10f;
            t -= (int)t;
            if ((10f * t) % 10 == (m_ArmNumber % 10)) {
                Shoot();
            }
        }
    }

    void Update() {
        ProcessInputs();
        //ProcessMouseInput();

        m_LineRend.enabled = m_LineRendEnabled;
        if (m_LineRendEnabled) {
            var line_length = 20f;
            UpdateLineRendererPositions(m_GunSource.position, m_GunSource.position + (line_length * ShootDirection));
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

    private void deprecated_ProcessMouseInput() {
        var gun_pos = PositionInScreeenSpace();
        var mouse_pos = Mouse.current.position.ReadValue();

        var screen_dimensions = new Vector3(Screen.width, Screen.height);
        gun_pos = Util.DivideElementWise2(gun_pos, screen_dimensions);
        mouse_pos = Util.DivideElementWise2(mouse_pos, screen_dimensions);

        var dist = Vector2.Distance(gun_pos, mouse_pos);
        if (m_ArmNumber == 1) {
            var cam = GameManager.Instance.CurrentCamera;
            Vector3 target = cam.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, 1));
            Debug.DrawLine(m_GunSource.position, target);
            if (m_DebugText) {
                m_DebugText.text = "Dist: " + Mathf.Round(dist * 100f) / 100f;
            }
        }

        m_InRange = dist <= DISTANCE_THRES;
        m_LineRendEnabled = m_InRange;
    }

    public void SetHighlighted(bool highlighted) {
        m_InRange = highlighted;
        var mat_prim = highlighted ? m_HighlightMaterial : m_RegularMaterial;
        var mat_sec = highlighted ? m_HighlightMaterialSecondary : m_RegularMaterialSecondary;
        var mats = m_ArmRenderer.materials;
        mats[0] = mat_prim;
        mats[1] = mat_sec;
        m_ArmRenderer.materials = mats;
        m_LineRendEnabled = highlighted;
    }

    public void Shoot() {
        m_GunshotAudio.PlayOneShot(m_GunshotAudio.clip);
        m_MuzzleFlash.Trigger();

        ForceField.DisableAllForSeconds(1.5f);

        var force = (-ShootDirection) * m_GunStrength;
        if (force.y < m_MinUpwardsForce) {
            force.y = m_MinUpwardsForce;
        }
        m_Rigidbody.AddForce(force, ForceMode.Impulse);

        var body_force_factor = 5f;
        float n = m_FishBodyRigidbodies.Count;
        foreach (var rb in m_FishBodyRigidbodies) {
            rb.AddForce(body_force_factor * force / n, ForceMode.Impulse);
        }
    }

    public Vector2 PositionInScreeenSpace() {
        var camera = GameManager.Instance.CurrentCamera;
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
