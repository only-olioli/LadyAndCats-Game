using UnityEngine.InputSystem;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private CharacterController controller;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private Vector3 _velocity;
    private bool _isGrounded;

    private InputAction _movement;

    private void Start()
    {
        _movement = new InputAction("movement");
        _movement.AddCompositeBinding(
            "",
            ) ;
    }

    private void Update()
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
