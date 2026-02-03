using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Настройки пули")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    
    private Rigidbody rb;
    private bool hasHitGround = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        if (!hasHitGround && rb.velocity.magnitude < 0.1f)
        {
            if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
            {
                hasHitGround = true;
                Destroy(gameObject, 1f);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasHitGround = true;
            Destroy(gameObject, 1f);
        }
    }
}