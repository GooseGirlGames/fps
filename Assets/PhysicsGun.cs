using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsGun : MonoBehaviour {

    [SerializeField]
    private List<InputActionReference> m_ShootActions = new List<InputActionReference>();

    [SerializeField]
    private Vector3 m_ShootDirection;

    void Start() {
        m_ShootDirection.Normalize();
    }

    void Update() {
        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + 5f * m_ShootDirection);
    }
}
