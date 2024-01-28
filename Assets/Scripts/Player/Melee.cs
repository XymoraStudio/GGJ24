using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Melee")]
    float range = 2;
    float knockbackForce = 500;
    [SerializeField] Transform raySpawnPoint; // From which point we cast ray
    [SerializeField] Animator handAnimator;

    private Material[] materials;
    [SerializeField] private CameraShake cameraShake;
    bool canAttack = true;
    Worker attackTarget;

    void Update()
    {
        // We update melee state
        if (Input.GetKeyDown(KeyBindsPlayer.attack) && canAttack)
        {
            MeleeAttack();
        }

        HighlightWorker();
    }


    private void MeleeAttack()
    {
        handAnimator.SetTrigger("Slap");
        RaycastHit hit;
        if(Physics.Raycast(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward), out hit, range)) {
            //Debug.DrawRay(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if(hit.collider.tag == "OfficeWorker") {
                attackTarget = hit.collider.transform.parent.GetComponent<Worker>();
            }
            else {
                attackTarget = null;
            }
        }
        canAttack = false;
    }
    void HighlightWorker() {
        // We check if we have to highlight office worker
        RaycastHit hit;
        if(Physics.Raycast(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward), out hit, range)) {
            if(hit.collider.tag == "OfficeWorker") {
                materials = hit.collider.GetComponent<SkinnedMeshRenderer>().materials;
                foreach(Material material in materials) {
                    material.SetColor("_OutlineColor", Color.white);
                }
            }
        }
        else {
            if(materials != null) {
                if(materials[0] != null) {
                    for(int i = 0; i < materials.Length; i++) {
                        materials[i].SetColor("_OutlineColor", Color.black);
                        materials[i] = null;
                    }
                }
            }
        }
    }
    private void AnimationRegisterAttack()
    {
        if(attackTarget != null) {
            Debug.Log("slap");
            attackTarget.SlapWorker();
            attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * knockbackForce);
            cameraShake.RestartShake(0.02f);
        }
        
    }
    void AttackAnimationOver() {
        Debug.Log("over slap");
        canAttack = true;
    }
}
