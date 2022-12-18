using UnityEngine;

public class ForceField : MonoBehaviour {
    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private float m_Strength = 1f;

    void OnTriggerStay(Collider other) {
        var rb = other.attachedRigidbody;
        if (rb && other.gameObject.CompareTag("Player")) {
            rb.AddForce(m_Strength * Direction(rb) * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    private Vector3 Direction(Rigidbody source) {
        var direction = m_Target.position - source.position;
        direction.Normalize();
        return direction;
    }
}
