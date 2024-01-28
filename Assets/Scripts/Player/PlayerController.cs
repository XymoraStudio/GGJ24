using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    const float DASH_STRENGTH = 100f;
    const float DASH_RESTART_TIME = 4f;
    private float timerDash;
    [SerializeField] private Image cooldownImage;

    Vector3 movementInput;

    private void Start() {
        timerDash = DASH_RESTART_TIME;
    }

    private void Update() {
        RotatingThePlayer();
        PlayerMoving();
        cameraTransform.position = playerCameraAttach.position;
        cameraTransform.rotation = playerCameraAttach.rotation;
        if(timerDash < DASH_RESTART_TIME){
            timerDash -= Time.deltaTime;
            if(timerDash <= 0){
                timerDash = DASH_RESTART_TIME;
                cooldownImage.fillAmount = 0;
            }
        }
    }
    void PlayerMoving() {
        float inputY = movementInput.y;
        Vector3 inputX = transform.right * Input.GetAxis("Horizontal");
        Vector3 inputZ = transform.forward * Input.GetAxis("Vertical");
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
            if(Input.GetKeyDown(KeyBindsPlayer.dash) && DASH_RESTART_TIME == timerDash) {
                timerDash -= Time.deltaTime;
                cooldownImage.transform.localScale = Vector3.one;
                cooldownImage.transform.DOScaleY(0, DASH_RESTART_TIME);
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
