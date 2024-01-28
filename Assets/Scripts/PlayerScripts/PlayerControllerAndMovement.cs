using UnityEngine;

public class PlayerControllerAndMovement : MonoBehaviour {
    [Header ("Camera")]
    [SerializeField] private float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header ("Movement")]
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] private float sprintStrenght = 2f;
    [SerializeField] private float gravityStrenght = 2f;
    [SerializeField] private float jumpStrenght = 2f;

    [Header ("Abilities")]
    [SerializeField] private float dashStrenght = 5f;
    private Vector3 movementInput;
    private CharacterController cc;

    private void Awake() {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatingThePlayer();
        PlayerMoving();
    }

    void PlayerMoving(){
        float inputY = movementInput.y;
        Vector3 inputX = transform.right * Input.GetAxis("Horizontal");
        Vector3 inputZ = transform.forward * Input.GetAxis("Vertical");
        movementInput = inputX + inputZ;
        if (movementInput.magnitude > 1) // da strejfanje ne bude brze
        {
            movementInput = movementInput.normalized;
        }
        if(Input.GetKey(KeyBinds.sprintKey)){
            movementInput *= sprintStrenght;
        }
        else{
            movementInput *= movementSpeed;
        }
        movementInput.y = inputY;
        if(cc.isGrounded){
            movementInput.y = Physics.gravity.y * gravityStrenght * Time.deltaTime;
            if(Input.GetKeyDown(KeyBinds.dash)){
                movementInput *= dashStrenght;
            }
        }
        else{
            movementInput.y += Physics.gravity.y * gravityStrenght * Time.deltaTime;
        }
        if(Input.GetKey(KeyBinds.jump) && cc.isGrounded){
            movementInput.y = jumpStrenght;
        }
        cc.Move(movementInput * Time.deltaTime);
    }

    //da kamera snapa unutar granica ako prede input granice
    void CameraCorrecting(ref Vector3 cameraNewRotation){
        if(cameraNewRotation.x > 180 && cameraNewRotation.x < 360){
            if(cameraNewRotation.x < 275)
                cameraNewRotation.x = 275;
        }
        else{
            if(cameraNewRotation.x > 85 && cameraNewRotation.x < 360){
                cameraNewRotation.x = 85;
            }
        }
    }

    void RotatingThePlayer()
    {
        //da se ne dogodi naopako gledanje
        Vector3 cameraRotation;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        cameraRotation = transform.rotation.eulerAngles + new Vector3(-mouseY, 0, 0);
        CameraCorrecting(ref cameraRotation);
        transform.rotation = Quaternion.Euler(cameraRotation);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, mouseX, 0));
    }
}
