using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float speedH;
    [SerializeField]
    private float speedV;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cam = Camera.main;

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            Camera.main.transform.forward = move;
        }

        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        yaw += speedH * inputManager.GetMouseDelta().x;
        pitch -= speedV * inputManager.GetMouseDelta().y;

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    
    }
}