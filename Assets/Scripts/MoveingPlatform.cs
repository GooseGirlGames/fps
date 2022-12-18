using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveingPlatform : MonoBehaviour {

    private List<Collider> m_CollidersInside = new List<Collider>();
    private bool m_PlayerIsParented = false;

    private bool m_Move = false;
    private Vector3 m_StartPos;

    [SerializeField]
    private Joint m_Joint;

    [SerializeField]
    private TMP_Text m_DebugText;

    private float m_MaxForce = 0f;

    void Awake() {
        m_StartPos = transform.position;
    }

    void Update() {
        if (m_Move) {
            // transform.position += Vector3.up * 1f * Time.deltaTime;
        }
        if (m_DebugText) {
            m_DebugText.text = "Force: " + m_Joint.currentForce.magnitude + "\n"
                + "Max: " + m_MaxForce;
        }
        m_MaxForce = Mathf.Max(m_MaxForce, m_Joint.currentForce.magnitude);
    }

    IEnumerator StartMovingCoroutine() {
        yield return new WaitForSeconds(1.2f);
        m_Move = true;
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!m_CollidersInside.Contains(other)) {
                m_CollidersInside.Add(other);
            }
            CheckParenting();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (m_CollidersInside.Contains(other)) {
                m_CollidersInside.Remove(other);
            }
            CheckParenting();
        }
    }

    void CheckParenting() {
        if (!m_PlayerIsParented && m_CollidersInside.Count > 0) {
            Reparent(enter: true);
            StartCoroutine(StartMovingCoroutine());
        } else if (m_PlayerIsParented && m_CollidersInside.Count == 0) {
            Reparent(enter: false);
            StopAllCoroutines();
            m_Move = false;
            transform.position = m_StartPos;
        }
    }

    void Reparent(bool enter) {
        var player = GameManager.Instance.PlayerRootRigidbody;
        var connected = enter ? player : null;
        m_Joint.connectedBody = connected;
        m_PlayerIsParented = enter;
    }
}
