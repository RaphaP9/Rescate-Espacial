using UnityEngine;
using Lofelt.NiceVibrations;

public class OptionsUIVibrationHandler : UIVibrationHandler
{
    [Header("Settings")]
    [SerializeField] private HapticPreset musicSliderHapticPreset;
    [SerializeField] private HapticPreset SFXSliderHapticPreset;
    [SerializeField] private HapticPreset dialoguesSliderHapticPreset;
    [Space]
    [SerializeField] private HapticPreset vibrationToggleHapticPreset;

    private void OnEnable()
    {
        SFXVolumeSliderUIHandler.OnSFXSliderDragEnd += SFXVolumeSliderUIHandler_OnSFXSliderDragEnd;
        SFXVolumeSliderUIHandler.OnSFXSliderPointerUp += SFXVolumeSliderUIHandler_OnSFXSliderPointerUp;

        MusicVolumeSliderUIHandler.OnMusicSliderDragEnd += MusicVolumeSliderUIHandler_OnMusicSliderDragEnd;
        MusicVolumeSliderUIHandler.OnMusicSliderPointerUp += MusicVolumeSliderUIHandler_OnMusicSliderPointerUp;

        DialoguesVolumeSliderUIHandler.OnDialoguesSliderDragEnd += DialoguesVolumeSliderUIHandler_OnDialoguesSliderDragEnd;
        DialoguesVolumeSliderUIHandler.OnDialoguesSliderPointerUp += DialoguesVolumeSliderUIHandler_OnDialoguesSliderPointerUp;

        VibrationToggleUIHandler.OnVibrationToggled += VibrationToggleUIHandler_OnVibrationToggled;
    }

    private void OnDisable()
    {
        SFXVolumeSliderUIHandler.OnSFXSliderDragEnd -= SFXVolumeSliderUIHandler_OnSFXSliderDragEnd;
        SFXVolumeSliderUIHandler.OnSFXSliderPointerUp -= SFXVolumeSliderUIHandler_OnSFXSliderPointerUp;

        MusicVolumeSliderUIHandler.OnMusicSliderDragEnd -= MusicVolumeSliderUIHandler_OnMusicSliderDragEnd;
        MusicVolumeSliderUIHandler.OnMusicSliderPointerUp -= MusicVolumeSliderUIHandler_OnMusicSliderPointerUp;

        DialoguesVolumeSliderUIHandler.OnDialoguesSliderDragEnd -= DialoguesVolumeSliderUIHandler_OnDialoguesSliderDragEnd;
        DialoguesVolumeSliderUIHandler.OnDialoguesSliderPointerUp -= DialoguesVolumeSliderUIHandler_OnDialoguesSliderPointerUp;

        VibrationToggleUIHandler.OnVibrationToggled -= VibrationToggleUIHandler_OnVibrationToggled;
    }

    #region Subscriptions
    private void MusicVolumeSliderUIHandler_OnMusicSliderPointerUp(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(musicSliderHapticPreset);
    }

    private void MusicVolumeSliderUIHandler_OnMusicSliderDragEnd(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(musicSliderHapticPreset);

    }

    private void SFXVolumeSliderUIHandler_OnSFXSliderPointerUp(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(SFXSliderHapticPreset);

    }

    private void SFXVolumeSliderUIHandler_OnSFXSliderDragEnd(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(SFXSliderHapticPreset);
    }

    private void DialoguesVolumeSliderUIHandler_OnDialoguesSliderPointerUp(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(dialoguesSliderHapticPreset);
    }

    private void DialoguesVolumeSliderUIHandler_OnDialoguesSliderDragEnd(object sender, System.EventArgs e)
    {
        PlayHaptic_Unforced(dialoguesSliderHapticPreset);
    }

    private void VibrationToggleUIHandler_OnVibrationToggled(object sender, VibrationToggleUIHandler.OnVibrationToggledEventArgs e)
    {
        if(e.isOn) PlayHaptic_Unforced(vibrationToggleHapticPreset);
    }
    #endregion
}
