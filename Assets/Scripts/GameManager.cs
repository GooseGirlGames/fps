using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;

    public CinemachineBrain CMBrain = null;
    public Camera CurrentCamera = null;
    public Respawn Respawn;

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
        // DontDestroyOnLoad(this);
    }

    void SetupRigidbodies() {
        foreach (var rb in FindObjectsOfType<Rigidbody>()) {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SetupCameraStuff();
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

    void OnEnable() {
        Awake(); // hope this won't break anything D:
        SetupCameraStuff();
        SetupRigidbodies();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
