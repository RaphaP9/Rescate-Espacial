using System;
using UnityEngine;

public class DialoguesVolumeSliderUIHandler : VolumeSliderUIHandler
{
    public static event EventHandler OnDialoguesSliderDragEnd;
    public static event EventHandler OnDialoguesSliderPointerUp;

    protected override void OnEnable()
    {
        base.OnEnable();
        DialoguesVolumeManager.OnDialoguesVolumeManagerInitialized += DialoguesVolumeManager_OnDialoguesVolumeManagerInitialized;
        DialoguesVolumeManager.OnDialoguesVolumeChanged += DialoguesVolumeManager_OnDialoguesVolumeChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DialoguesVolumeManager.OnDialoguesVolumeManagerInitialized -= DialoguesVolumeManager_OnDialoguesVolumeManagerInitialized;
        DialoguesVolumeManager.OnDialoguesVolumeChanged -= DialoguesVolumeManager_OnDialoguesVolumeChanged;
    }

    protected override VolumeManager GetVolumeManager() => DialoguesVolumeManager.Instance;
    protected override void OnDragEndMethod() => OnDialoguesSliderDragEnd?.Invoke(this, EventArgs.Empty);
    protected override void OnPointerUpMethod() => OnDialoguesSliderPointerUp?.Invoke(this, EventArgs.Empty);

    private void DialoguesVolumeManager_OnDialoguesVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DialoguesVolumeManager_OnDialoguesVolumeChanged(object sender, VolumeManager.OnVolumeChangedEventArgs e)
    {
        //UpdateVisual();
    }
}
