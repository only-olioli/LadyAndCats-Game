using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Bindings bindings;

    private Movement scriptMovement;
    private LookWithMouse scriptLookWithMouse;
    private Interaction scriptInteraction;

    public enum Active { 
        enable,
        onlyLook,
        onlyMove,
        disable
    }
    public Active active;

    public bool isInteractive;

    private void Start()
    {
        scriptMovement = GetComponent<Movement>();
        scriptLookWithMouse = GetComponentInChildren<LookWithMouse>();

        scriptMovement.movement = new InputAction("movement");
        scriptMovement.movement.AddCompositeBinding("Dpad")
            .With("Up", bindings.up)
            .With("Down", bindings.down)
            .With("Left", bindings.left)
            .With("Right", bindings.right);

        scriptMovement.jump = new InputAction("jump");
        scriptMovement.jump.AddBinding(bindings.jump);

        scriptMovement.run = new InputAction("run");
        scriptMovement.run.AddBinding(bindings.shift);

        scriptMovement.BindingsActivate();
    }

    private void Update()
    {
        if (active == Active.enable) {
            scriptMovement.Move();
            scriptLookWithMouse.Look();
        }
        else if(active == Active.onlyMove) {
            scriptMovement.Move(); 
        }
        else if (active == Active.onlyLook)
        {
            scriptLookWithMouse.Look();
        }
    }
}
