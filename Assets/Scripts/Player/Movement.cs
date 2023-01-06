using UnityEngine.InputSystem;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private CharacterController controller;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private Vector3 velocity;
    private bool isGrounded;

    [SerializeField] private InputActionAsset inputAsset;
    private InputAction movement;
    private InputAction jump;

    void Start()
    {
        movement = inputAsset.FindAction("KeyboardDelta");

        jump = inputAsset.FindAction("jump");

        movement.Enable();
        jump.Enable();
    }

    void Update()
    {
        float x;
        float z;
        bool jumpPressed = false;

        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
        jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
