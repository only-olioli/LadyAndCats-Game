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
    }

    private void Update()
    {
        float x;
        float z;
        bool jumpPressed = false;

        var delta = _movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
        jumpPressed = Mathf.Approximately(Keyboard.current.spaceKey.ReadValue(), 1);

        _isGrounded = controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (jumpPressed && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }
}
