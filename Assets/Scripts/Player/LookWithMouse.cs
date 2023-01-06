using UnityEngine.InputSystem;
using UnityEngine;

public class LookWithMouse : MonoBehaviour
{
    const float k_MouseSensitivityMultiplier = 0.01f;

    public float mouseSensitivity;

    private bool isCutSceneNow;

    [SerializeField] private CutScene cutScene;
    [SerializeField] private Transform playerBody;
    [SerializeField] private InputActionAsset inputAsset;
    float xRotation = 0f;

    private InputAction mouseDelta;

    void Start()
    {
        mouseDelta = inputAsset.FindAction("MouseDelta");
        mouseDelta.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isCutSceneNow = cutScene.cutSceneNow;
    }

    void Update()
    {
        if (!isCutSceneNow)
        {
            bool unlockPressed = false, lockPressed = false;
            float mouseX = 0, mouseY = 0;

            if (Mouse.current != null)
            {
                var delta = mouseDelta.ReadValue<Vector2>() / 15.0f;
                mouseX += delta.x;
                mouseY += delta.y;
                lockPressed = Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame;
            }
            if (Gamepad.current != null)
            {
                var value = Gamepad.current.rightStick.ReadValue() * 2;
                mouseX += value.x;
                mouseY += value.y;
            }
            if (Keyboard.current != null)
            {
                unlockPressed = Keyboard.current.escapeKey.wasPressedThisFrame;
            }

            mouseX *= mouseSensitivity * k_MouseSensitivityMultiplier;
            mouseY *= mouseSensitivity * k_MouseSensitivityMultiplier;

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
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}
