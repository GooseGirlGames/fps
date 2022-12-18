using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TentacleSelect : MonoBehaviour {
    [SerializeField]
    private LayerMask m_TentacleLayer;

    private List<PhysicsGun> m_Guns;

    void Start() {
        m_Guns = new List<PhysicsGun>(FindObjectsOfType<PhysicsGun>());
    }

    void Update() {
        var targetgun = GetRayCastTarget();
        foreach (var gun in m_Guns) {
            gun.SetHighlighted(gun == targetgun);
        }
    }

    private PhysicsGun GetRayCastTarget() {
        var cam = GameManager.Instance.CurrentCamera;
        var mouse_pos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit hit;
        bool was_hit = Physics.Raycast(ray, out hit, m_TentacleLayer);
        if (!was_hit) {
            return null;
        }
        if (!hit.collider) {
            return null;
        }
        var go = hit.collider.gameObject;
        var gun = go.GetComponentInChildren<PhysicsGun>();

        if (gun) {
            return gun;
        }

        // Allow clicking on the gun model
        var parent = go.transform.parent;
        if (parent) {
            return parent.GetComponent<PhysicsGun>();
        }

        return null;
    }
}
