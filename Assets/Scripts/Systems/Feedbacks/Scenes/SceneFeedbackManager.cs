using UnityEngine;
using UnityEngine.InputSystem;

public class SceneFeedbackManager : MonoBehaviour
{
    public static SceneFeedbackManager Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
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
            //Debug.LogWarning("There is more than one SceneFeedbackManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
