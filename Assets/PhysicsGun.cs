using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsGun : MonoBehaviour {

    [SerializeField]
    private InputActionReference m_ShootAction;

    [SerializeField]
    private Transform m_GunSource;

    [SerializeField]
    [Range(0.01f, 20f)]
    private float m_GunStrength;

    private Rigidbody m_Rigidbody;


    public Vector3 ShootDirection {
        get => m_GunSource.forward;
    }

    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        ProcessInputs();
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_GunSource.position, m_GunSource.position + 5f * ShootDirection);
    }

    private void ProcessInputs() {
        if (m_ShootAction.action.WasPressedThisFrame()) {
            Shoot();
        }
    }

    private void Shoot() {
        Debug.Log("Boom");
        m_Rigidbody.AddForce((-ShootDirection) * m_GunStrength, ForceMode.Impulse);
    }
}
