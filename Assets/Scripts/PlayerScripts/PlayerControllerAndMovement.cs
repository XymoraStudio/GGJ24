using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAndMovement : MonoBehaviour
{
    public static PlayerControllerAndMovement instance;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float movementSpeed = 1.5f;
    public float sprintStrenght = 2f;
    public float gravityStrenght = 2f;
    Vector3 movementInput;
    CharacterController cc;

    private void Awake() {
        instance = this;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatingThePlayer();
        PlayerMoving();
    }

    void PlayerMoving(){
        Vector3 inputX = transform.right * Input.GetAxis("Horizontal");
        Vector3 inputZ = transform.forward * Input.GetAxis("Vertical");
        movementInput = inputX + inputZ;
        if (movementInput.magnitude > 1) // da strejfanje ne bude brze
        {
            movementInput = movementInput.normalized;
        }
        if(Input.GetKey(KeyBinds.instance.sprintKey)){
            movementInput *= sprintStrenght;
        }
        else{
            movementInput *= movementSpeed;
        }
        if(cc.isGrounded){
            movementInput.y = Physics.gravity.y * gravityStrenght * Time.deltaTime;
        }
        else{
            movementInput.y += Physics.gravity.y * gravityStrenght * Time.deltaTime;
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
