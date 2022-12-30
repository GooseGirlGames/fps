using System.Collections;
using UnityEngine;

// sorry about the typo, too lazy to fix :(
public class MoveingPlatform : MonoBehaviour {

    private bool m_PlayerInside = false;
    private bool m_PlayerIsParented = false;

    private bool m_Move = false;
    private Vector3 m_StartPos;
    private Quaternion m_StartRot;

    private float m_MaxHeight = 73f;

    [SerializeField]
    private Transform m_Origin;  // transform to move around

    [SerializeField]
    private float m_Speed = 4f;

    private Coroutine m_ChangeMovingCoroutine = null;

    private bool m_RentryBlocked = false;

    void Awake() {
        m_StartPos = m_Origin.position;
        m_StartRot = m_Origin.rotation;
    }

    void ResetPositon() {
        m_Origin.position = m_StartPos;
        m_Origin.rotation = m_StartRot;
    }

    /*
    public bool m_X;
    public bool m_Y;
    public bool m_Z;
    private Vector3 Axis() {
        return new Vector3(m_X?1:0, m_Y?1:0, m_Z?1:0);
    }
    */

    void FixedUpdate() {
        var origin_rb = m_Origin.GetComponent<Rigidbody>();
        var height = origin_rb.position.y;
        Debug.Log(height);
        if (m_Move && height < m_MaxHeight) {
            var delta_t = Time.fixedDeltaTime;

            var phi = -3.5f * delta_t * m_Speed;
            var axis = new Vector3(0, 1, 0);
            var euler = axis * phi;
            var rot = Quaternion.Euler(euler.x, euler.y, euler.z);
            // m_Origin.rotation *= rot;
            origin_rb.MoveRotation(origin_rb.rotation * rot);

            var dx = 0.35f * delta_t * m_Speed;
            var dy = 1.45f * delta_t * m_Speed;
            var delta_pos = new Vector3(dx, dy, 0);
            // m_Origin.Translate(delta_pos);
            origin_rb.MovePosition(origin_rb.position + delta_pos);


            // var player_rb = GameManager.Instance.PlayerRootRigidbody;
            // player_rb.MovePosition(m_PlatformCenter.position);
        }
    }

    IEnumerator BlockReentryCoroutine(float t) {
        m_RentryBlocked = true;
        yield return new WaitForSeconds(t);
        m_RentryBlocked = false;
    }

    IEnumerator ChangeMovingCoroutine(bool move, float t) {
        Debug.Log("Will set move to " + move + " soon.");
        yield return new WaitForSeconds(t);
        m_Move = move;
        Debug.Log("Move set to " + move + ".");
    }

    void StartChangeMovingCoroutine(bool move, float t = 2f) {
        if (m_ChangeMovingCoroutine != null) {
            StopCoroutine(m_ChangeMovingCoroutine);
        }
        m_ChangeMovingCoroutine = StartCoroutine(ChangeMovingCoroutine(move, t));
    }


    void OnTriggerEnter(Collider other) {
        if (m_RentryBlocked) {
            return;
        }

        if (other.gameObject.CompareTag("Player")) {
            if (other.gameObject.GetComponent<Rigidbody>() == GameManager.Instance?.PlayerRootRigidbody) {
                Debug.Log("Player entered");
                m_PlayerInside = true;
                CheckParenting();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (other.gameObject == GameManager.Instance?.PlayerRootRigidbody) {
                m_PlayerInside = false;
                CheckParenting();
            }
        }
    }

    void CheckParenting() {
        if (!m_PlayerIsParented && m_PlayerInside) {
            Reparent(enter: true);
            StartChangeMovingCoroutine(move: true);
        } else if (m_PlayerIsParented && !m_PlayerInside) {
            Reparent(enter: false);
            StartChangeMovingCoroutine(move: false);
            // StopAllCoroutines();  ?!?!?!?!
            m_Move = false;
            ResetPositon();
        }
    }

    public void Reparent(bool enter, bool leaving_from_gun = false) {
        if (leaving_from_gun) {
            StartCoroutine(BlockReentryCoroutine(2f));
        }

        var player = GameManager.Instance.PlayerObject.transform;
        foreach (var gun in player.GetComponentsInChildren<PhysicsGun>()) {
            gun.CurrentMovingPlatform = enter ? this : null;
        }
        var parent = enter ? this.transform : null;
        //foreach (var rb in player.GetComponentsInChildren<Rigidbody>()) {
        //    rb.isKinematic = enter;
        //}
        var rb = GameManager.Instance.PlayerRootRigidbody;
        rb.isKinematic = enter;
        player.SetParent(parent, worldPositionStays: true);

        m_PlayerIsParented = enter;
    }
}
