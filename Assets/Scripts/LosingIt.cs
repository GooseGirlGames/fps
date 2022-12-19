using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LosingIt : MonoBehaviour {
    public const float LOSING_IT_THRESHOLD = 15f;
    public TMP_Text DebugText;

    private List<Transform> m_Arms = new List<Transform>();

    void Awake() {
        foreach (var gun in FindObjectsOfType<PhysicsGun>()) {
            m_Arms.Add(gun.transform);
        }
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    void Start() {
        StartCoroutine(CheckLostCoroutine());
    }

    public IEnumerator CheckLostCoroutine() {
        while (true) {
            var l = Lostness();
            if (l >= LOSING_IT_THRESHOLD) {
                WeAreLosingIt();
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public void WeAreLosingIt() {
        Debug.LogError("Octopus is gone");
        GameManager.Instance.Respawner.RestartLevel();
    }

    public float Lostness() {
        float dist = 0f;
        foreach (var arm in m_Arms) {
            dist += Vector3.Distance(arm.position, m_Arms[0].position);
        }
        return dist/ (m_Arms.Count - 1);
    }
}
