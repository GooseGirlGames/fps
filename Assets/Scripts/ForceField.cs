using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ForceField : MonoBehaviour {
    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private float m_Strength = 1f;

    private static List<ForceField> s_Fields = new List<ForceField>();

    private bool m_ForceEnabled = true;

    void OnEnable() {
        s_Fields.Add(this);
    }
    void OnDisable() {
        s_Fields.Remove(this);
    }

    public static void DisableAllForSeconds(float secs) {
        foreach (var field in s_Fields) {
            field.DisableForSeconds(secs);
        }
    }

    private void DisableForSeconds(float secs) {
        StopAllCoroutines();
        StartCoroutine(DisableCoroutine(secs));
    }

    IEnumerator DisableCoroutine(float for_seconds) {
        m_ForceEnabled = false;
        yield return new WaitForSeconds(for_seconds);
        m_ForceEnabled = true;
    }

    void OnTriggerStay(Collider other) {
        if (!m_ForceEnabled) {
            return;
        }

        var rb = other.attachedRigidbody;
        if (rb && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Among"))) {
            rb.AddForce(m_Strength * Direction(rb) * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    private Vector3 Direction(Rigidbody source) {
        var direction = m_Target.position - source.position;
        direction.Normalize();
        return direction;
    }
}
