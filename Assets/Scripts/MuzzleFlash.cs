using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    [SerializeField]
    private ParticleSystem m_ParticleSystem;

    void Start() {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_ParticleSystem.Stop();
    }

    public void Trigger() {
        m_ParticleSystem.Emit(1000);
    }
}
