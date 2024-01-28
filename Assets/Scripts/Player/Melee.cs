using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Melee")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float range;
    [SerializeField] private Transform raySpawnPoint; // From which point we cast ray
    [SerializeField] private Animator handAnimator;

    private State state;
    private float lastAttackTime = 0;

    enum State
    {
        Melee,
        Idle
    }

    void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        // We update melee state
        if (Input.GetKeyDown(KeyBindsPlayer.attack))
        {
            if (state == State.Idle) state = State.Melee;
        }

        if (Input.GetKeyUp(KeyBindsPlayer.attack))
        {
            if (state == State.Melee) state = State.Idle;
        }

        // We use melee state for decisions
        if (state == State.Melee)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (Time.time - lastAttackTime > cooldown) // Enough time have passed to attack again
        {
            handAnimator.SetTrigger("Slap");

            RaycastHit hit;
            if (Physics.Raycast(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward), out hit, range))
            {
                if (hit.collider.tag == "OfficeWorker")
                    hit.collider.transform.parent.GetComponent<Worker>().SlapWorker();
                Debug.DrawRay(raySpawnPoint.position, raySpawnPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                lastAttackTime = Time.time;
            }
        }
    }
}
