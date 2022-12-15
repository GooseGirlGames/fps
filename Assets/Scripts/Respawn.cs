using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {

    [SerializeField]
    private Scene m_Level1;

    private int m_CurrentLevel = 1;

    void Awake() {
        DontDestroyOnLoad(this);
    }

    public void LoadLevel(int level) {
        if (level == 1) {
            SceneManager.LoadScene(m_Level1.name);
        }
        m_CurrentLevel = level;
    }

    public void RestartLevel() {
        Debug.Log("RestartLevel");
        LoadLevel(m_CurrentLevel);
    }
}
