using UnityEngine;

public class OptionsUISFXHandler : UISFXHandler
{
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
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void MusicVolumeSliderUIHandler_OnMusicSliderDragEnd(object sender, System.EventArgs e)
    {
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void SFXVolumeSliderUIHandler_OnSFXSliderPointerUp(object sender, System.EventArgs e)
    {
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void SFXVolumeSliderUIHandler_OnSFXSliderDragEnd(object sender, System.EventArgs e)
    {
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void DialoguesVolumeSliderUIHandler_OnDialoguesSliderPointerUp(object sender, System.EventArgs e)
    {
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void DialoguesVolumeSliderUIHandler_OnDialoguesSliderDragEnd(object sender, System.EventArgs e)
    {
        PlaySFX_Unpausable(SFXPool.sliderRelease);
    }

    private void VibrationToggleUIHandler_OnVibrationToggled(object sender, VibrationToggleUIHandler.OnVibrationToggledEventArgs e)
    {
        if(e.isOn) PlaySFX_Unpausable(SFXPool.toggleReleaseOn);
        else PlaySFX_Unpausable(SFXPool.toggleReleaseOff);
    }
    #endregion
}
