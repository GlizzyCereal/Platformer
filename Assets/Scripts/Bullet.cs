using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 20;
    public float damageMin = 0.5f;
    public float damageMax = 2;
    public float lifetime = 3;

    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();    
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        var damage = Random.Range(damageMin, damageMax);
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage((int)damage);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }
}
