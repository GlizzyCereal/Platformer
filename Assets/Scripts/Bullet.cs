using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damageMin = 0.5f;
    public float damageMax = 2f;
    public float lifetime = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();    
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        float damage = Random.Range(damageMin, damageMax);
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}