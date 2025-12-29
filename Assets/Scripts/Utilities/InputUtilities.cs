using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class InputUtilities
{
    private const float LANDSCAPE_THRESHOLD = 0.7f;

    public static Vector2 GetScreenDimensions() => new Vector2(Screen.width, Screen.height);
    public static float GetScreenAspect()
    {
        return GetScreenDimensions().x / GetScreenDimensions().y;
    }

    public static Vector2 GetPointerPosition()
    {
        // Using Mobile or Emulator
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) return Touchscreen.current.primaryTouch.position.ReadValue();
        // Mouse input
        if (Mouse.current != null) return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }

    //Returns True if screen tap/clicked on this frame
    public static bool TryGetTapPosition(out Vector2 screenPosition)
    {
        // Mobile or Emulator
        if (Touchscreen.current != null)
        {
            TouchControl touchControl = Touchscreen.current.primaryTouch;

            if (touchControl.press.wasPressedThisFrame)
            {
                screenPosition = touchControl.position.ReadValue();
                return true;
            }
        }

        // Mouse
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPosition = Mouse.current.position.ReadValue();
            return true;
        }

        screenPosition = Vector2.zero;
        return false;
    }

    public static bool HasAccelerommeter() => Accelerometer.current != null;
    public static Vector3 GetAccelerommeterValue()
    {
        if(!HasAccelerommeter()) return Vector3.zero;

        Vector3 accel = Accelerometer.current.acceleration.ReadValue();
        return accel;
    }

    public static bool IsLandscapeLeft()
    {
        Vector3 accel = GetAccelerommeterValue();
        bool isLandscape = Mathf.Abs(accel.x) > Mathf.Abs(accel.y);

        return isLandscape && Mathf.Abs(accel.x) > LANDSCAPE_THRESHOLD;
    }
}
