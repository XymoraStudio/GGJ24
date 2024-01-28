using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Melee")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float range;
    [SerializeField] private Transform raySpawnPoint; // From which point we cast ray
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float knockbackForce;

    private State state;
    private float lastAttackTime = 0;
    private Material[] materials;
    private CameraShake cameraShake;

    enum State
    {
        Melee,
        Idle
    }

    void Start()
    {
        state = State.Idle;
        cameraShake = FindObjectOfType<CameraShake>();
    }

    void Update()
    {
        // We update melee state
        if (Input.GetKeyDown(KeyBinds.instance.attack))
        {
            if (state == State.Idle) state = State.Melee;
        }

        if (Input.GetKeyUp(KeyBinds.instance.attack))
        {
            if (state == State.Melee) state = State.Idle;
        }

        // We use melee state for decisions
        if (state == State.Melee)
        {
            MeleeAttack();
        }

        // We check if we have to highlight office worker
        RaycastHit hit;
        if (Physics.Raycast(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.collider.tag == "OfficeWorker") {
                materials = hit.collider.GetComponent<SkinnedMeshRenderer>().materials;
                foreach (Material material in materials)
                {
                    material.SetColor("_OutlineColor", Color.white);
                }
            }
        }
        else
        {
            if (materials != null)
            {
                if (materials[0] != null)
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].SetColor("_OutlineColor", Color.black);
                        materials[i] = null;
                    }
                }
            }
        }
    }


    private void MeleeAttack()
    {
        if (Time.time - lastAttackTime > cooldown) // Enough time have passed to attack again
        {
            handAnimator.SetBool("slap", true);
            Invoke("CheckAttack", 0.4f);
        }
    }

    private void CheckAttack()
    {
        handAnimator.SetBool("slap", false);
        RaycastHit hit;
        if (Physics.Raycast(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.collider.tag == "OfficeWorker")
            {
                hit.collider.transform.parent.GetComponent<EnemyActions>().active = false;
                hit.collider.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * knockbackForce);
                cameraShake.RestartShake(0.02f);
            }
            lastAttackTime = Time.time;
        }
    }
}
