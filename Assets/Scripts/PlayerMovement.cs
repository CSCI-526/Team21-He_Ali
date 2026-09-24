using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1f, 15f)]
    private float moveSpeed = 6f;

    private Rigidbody playerRigidbody;
    private float horizontalInput;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontalInput = 0f;

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalInput -= 1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalInput += 1f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement =
            Vector3.right *
            horizontalInput *
            moveSpeed *
            Time.fixedDeltaTime;

        playerRigidbody.MovePosition(
            playerRigidbody.position + movement
        );
    }

}
