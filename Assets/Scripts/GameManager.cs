using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;

    public CinemachineBrain CMBrain = null;
    public Camera CurrentCamera = null;
    public Respawn Respawner;

    [SerializeField]
    private TMP_Text m_DebugText;

    [HideInInspector]
    public GameObject PlayerObject = null;
    [HideInInspector]
    public Rigidbody PlayerRootRigidbody = null;


    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void SetupRigidbodies() {
        foreach (var rb in FindObjectsOfType<Rigidbody>()) {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SetupRigidbodies();
        SetupCameraStuff();
        Debug.Log("Scene Loaded, Respawn is " + Respawner);
        if (!Respawner) {
            Respawner = GetComponent<Respawn>();
        }
        if (Respawner) {
            Respawner.SetPlayerToSpawnLocation();
        } else {
            Debug.LogWarning("Scene loaded but no Respawn object found");
        }
    }

    public void SetupCameraStuff() {
        CMBrain = FindObjectOfType<CinemachineBrain>();
        var cams = FindObjectsOfType<Camera>();
        if (cams.Length != 1) {
            Debug.LogWarning($"Found {cams.Length} cameras.  Expected 1.");
        } else {
            CurrentCamera = cams[0];
        }
    }

    void Update() {
        if (m_DebugText != null) {
            m_DebugText.text = $"{PlayerObject}";
        }
    }

    void OnEnable() {
        SetupCameraStuff();
        SetupRigidbodies();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
