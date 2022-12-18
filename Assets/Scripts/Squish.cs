using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour {
    [SerializeField]
    private List<AudioClip> m_SquishSounds = new List<AudioClip>();

    [SerializeField]
    private float m_Cooldown = 3f;

    [SerializeField]
    private float m_Chance = 0.6f;

    private float m_LastPlayed = 0;

    [SerializeField]
    private bool m_CooldownOnLevelLoad = false;

    void Awake() {
        m_Source = GetComponent<AudioSource>();
    }

    private AudioSource m_Source;

    void OnCollisionEnter(Collision c) {
        if (UnityEngine.Random.Range(0f, 1f) < m_Chance) {
            PlaySquish();
        }
    }

    public void PlaySquish() {
        if (m_CooldownOnLevelLoad && Time.timeSinceLevelLoad <= m_Cooldown) {
            return;
        }

        if (m_Source.isPlaying) {
            return;
        }

        var time = Time.fixedTime;

        if (m_LastPlayed + m_Cooldown > time) {
            return;
        }

        var clip = Util.RandomChoice<AudioClip>(m_SquishSounds);
        m_Source.PlayOneShot(clip);
        m_LastPlayed = time;
    }
}
