using System;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneButtonUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button cutsceneButton;
    [SerializeField] private Image cutsceneThumbnailImage;
    [Space]
    [SerializeField] private Transform buttonUI;
    [SerializeField] private Transform notUnlockedCoverUI;

    [Header("Settings")]
    [SerializeField] private CutsceneSO cutsceneSO;
    [SerializeField] private bool enableButtonWhenNotUnlocked;

    private void Awake()
    {
        InitializeButtonsListeners();
        SetCutsceneThumbnail();
    }

    private void Start()
    {
        HandleNotUnlockedCover();
    }

    private void InitializeButtonsListeners()
    {
        cutsceneButton.onClick.AddListener(PlayCutscene);
    }

    private void SetCutsceneThumbnail()
    {
        cutsceneThumbnailImage.sprite = cutsceneSO.cutsceneThumbnail;
    }

    private void HandleNotUnlockedCover()
    {
        bool unlocked = DataContainer.Instance.HasUnlockedCutscene(cutsceneSO);

        if (unlocked)
        {
            buttonUI.gameObject.SetActive(true);
            notUnlockedCoverUI.gameObject.SetActive(false);
        }
        else
        {
            buttonUI.gameObject.SetActive(enableButtonWhenNotUnlocked);
            notUnlockedCoverUI.gameObject.SetActive(true);
        }
    }

    private void PlayCutscene() => AlbumSceneCutsceneUI.Instance.PlayCutscene(cutsceneSO);
}
