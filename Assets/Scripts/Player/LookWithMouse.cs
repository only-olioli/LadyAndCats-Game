using UnityEngine;
using UnityEngine.InputSystem;

public class LookWithMouse : MonoBehaviour
{
    public float mouseSensitivityMultiplier = 0.01f;

    public float mouseSensitivity;

    public bool isActive = true;

    [SerializeField] private Transform playerBody;
    private float _xRotation = 0f;

    void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isActive)
        {
            bool unlockPressed = false, lockPressed = false;
            float mouseX = 0, mouseY = 0;

            if (Mouse.current != null)
            {
                var delta = Mouse.current.delta.ReadValue() / 15f;
                mouseX += delta.x;
                mouseY += delta.y;
            }
            else {
                isActive = false;
            }
            if (Keyboard.current != null)
            {
                unlockPressed = Keyboard.current.escapeKey.wasPressedThisFrame;
            }

            mouseX *= mouseSensitivity * mouseSensitivityMultiplier;
            mouseY *= mouseSensitivity * mouseSensitivityMultiplier;

            if (unlockPressed)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (lockPressed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

                transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }else if(Mouse.current != null)
        {
            isActive = true;
        }
    }
}
