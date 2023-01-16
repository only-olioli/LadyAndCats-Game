using UnityEngine.InputSystem;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private CharacterController controller;

    public float speed = 5;
    public float runSpeed = 7;
    public float gravity = -10;
    public float jumpHeight = 0.8f;

    private Vector3 _velocity;
    private bool _isGrounded;

    public InputAction movement;
    public InputAction run;
    public InputAction jump;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void BindingsActivate()
    {
        movement.Enable();
        jump.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
    }

    public void Move()
    {
        float x;
        float z;
        float ActualSpeed;
        bool jumpPressed = false;
        bool runPressed = false;

        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
        jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);
        runPressed = Mathf.Approximately(run.ReadValue<float>(), 1);

        ActualSpeed = runPressed ? runSpeed : speed;

        _isGrounded = controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * ActualSpeed * Time.deltaTime);

        if (jumpPressed && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }
}
