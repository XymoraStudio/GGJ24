using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    private float speed;
    private float lifetime;
    private float damage;

    void Update()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
    }

    public void Initialize(float damage_, float speed_, float lifetime_)
    {
        damage = damage_;
        speed = speed_;
        lifetime = lifetime_;

        Destroy(gameObject, lifetime);
    }
}
