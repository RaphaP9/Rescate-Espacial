using UnityEngine;

public class FramerateManager : MonoBehaviour
{
    public static FramerateManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField,Range(30,120)] private int targetFramerate;
    [SerializeField] private bool useVSync;

    private const int UNLIMITED_FPS_NUMBER = -1;
    private const int VSYNC_ENABLED_NUMBER = 1;

    private void Awake()
    {
        SetSingleton();
        SetTargetFramerate();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one FramerateManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetTargetFramerate()
    {
        //If VSync is on, targetFramerate will be ignored

        #if !UNITY_EDITOR
        Application.targetFrameRate = targetFramerate;
        if (useVSync)
        {
            QualitySettings.vSyncCount = VSYNC_ENABLED_NUMBER;
        }
        #else
        Application.targetFrameRate = UNLIMITED_FPS_NUMBER;
        #endif
    }
}
