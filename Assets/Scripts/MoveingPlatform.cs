using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingPlatform : MonoBehaviour {

    private List<Collider> m_CollidersInside = new List<Collider>();
    private bool m_PlayerIsParented = false;

    private bool m_Move = false;
    private Vector3 m_StartPos;

    void Awake() {
        m_StartPos = transform.position;
    }

    void Update() {
        if (m_Move) {
            transform.position += Vector3.up * 1f * Time.deltaTime;
        }
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
        var player = GameManager.Instance.PlayerObject.transform;
        var parent = enter ? this.transform : null;
        player.SetParent(parent, worldPositionStays: true);
        m_PlayerIsParented = enter;
    }
}
