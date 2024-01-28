using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Camera")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform playerCameraAttach;

    [Header("Controller")]
    [SerializeField] CharacterController characterController;

    // Settings
    const float MOUSE_SENSITIVITY = 2f;
    const float MOVEMENT_SPEED = 5f;
    const float SPRINT_STRENGTH = 10f;
    const float GRAVITY_STRENGTH = 2f;
    const float JUMP_STRENGTH = 6f;
    const float DASH_STRENGTH = 10f;

    public bool movementBlocked = false;

    Vector3 movementInput;

    private void Update() {
        RotatingThePlayer();
        PlayerMoving();
        cameraTransform.position = playerCameraAttach.position;
        cameraTransform.rotation = playerCameraAttach.rotation;
    }

    private void Start()
    {
        movementBlocked = false;
    }

    void PlayerMoving() {
        if (movementBlocked) return;
        float inputY = movementInput.y;
        Vector3 inputX = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 inputZ = transform.forward * Input.GetAxisRaw("Vertical");
        movementInput = inputX + inputZ;
        movementInput = movementInput.normalized;

        // Sprint or walk
        if(Input.GetKey(KeyBindsPlayer.sprintKey)) {
            movementInput *= SPRINT_STRENGTH;
        }
        else {
            movementInput *= MOVEMENT_SPEED;
        }

        // Jump
        movementInput.y = inputY;

        if(characterController.isGrounded) {
            movementInput.y = Physics.gravity.y * GRAVITY_STRENGTH * Time.deltaTime;
            if(Input.GetKeyDown(KeyBindsPlayer.dash)) {
                movementInput *= DASH_STRENGTH;
            }
        }
        else {
            movementInput.y += Physics.gravity.y * GRAVITY_STRENGTH * Time.deltaTime;
        }
        if(Input.GetKey(KeyBindsPlayer.jump) && characterController.isGrounded) {
            movementInput.y = JUMP_STRENGTH;
        }

        characterController.Move(movementInput * Time.deltaTime);
    }

    //da kamera snapa unutar granica ako prede input granice
    void CameraCorrecting(ref Vector3 cameraNewRotation) {
        if(cameraNewRotation.x > 180 && cameraNewRotation.x < 360) {
            if(cameraNewRotation.x < 275)
                cameraNewRotation.x = 275;
        }
        else {
            if(cameraNewRotation.x > 85 && cameraNewRotation.x < 360) {
                cameraNewRotation.x = 85;
            }
        }
    }

    void RotatingThePlayer() {
        //da se ne dogodi naopako gledanje
        Vector3 cameraRotation;
        float mouseY = Input.GetAxisRaw("Mouse Y") * MOUSE_SENSITIVITY;
        float mouseX = Input.GetAxisRaw("Mouse X") * MOUSE_SENSITIVITY;
        cameraRotation = transform.rotation.eulerAngles + new Vector3(-mouseY, 0, 0);
        CameraCorrecting(ref cameraRotation);
        transform.rotation = Quaternion.Euler(cameraRotation);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, mouseX, 0));
    }
}
